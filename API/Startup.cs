using System;
using API.Domain;
using API.Domain.ConfigurationModels;
using API.Kafka;
using API.Kafka.BackgroundServices;
using API.Kafka.Consumers;
using API.Kafka.Producers;
using API.Services;
using API.Services.Interfaces;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using DataAccess;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddFluentValidation(options => {
                    options.RegisterValidatorsFromAssemblyContaining<Startup>();
                    options.ImplicitlyValidateChildProperties = true;
                    options.DisableDataAnnotationsValidation = true;
                });
            services.AddHostedService<ProcessOrdersBackgroundService>();


            services.AddDbContext<DataContext>(options => {
                options.UseSqlServer(DataAccessLibraryConfig.GetConnectionString());
            });


            var kafkaProducerConfig = new ProducerConfiguration();
            Configuration.GetSection("Kafka:Producers")
                .Bind(kafkaProducerConfig);
            services.AddSingleton(new ProducerConfig
            {
                BootstrapServers = kafkaProducerConfig.BootstrapServers,
                Acks = kafkaProducerConfig.Acks,
                MessageSendMaxRetries = kafkaProducerConfig.MessageSendMaxRetries,
                RetryBackoffMs = kafkaProducerConfig.RetryBackoffMs,
                EnableIdempotence = kafkaProducerConfig.EnableIdempotence,
            });

            services.AddSingleton(new SchemaRegistryConfig
            {
                Url = Configuration.GetValue<string>("Kafka:SchemaRegistry:Url")
            });
            var kafkaConsumerConfig = new ConsumerConfiguration();
            Configuration.GetSection("Kafka:Consumers")
                .Bind(kafkaConsumerConfig);
            services.AddSingleton(new ConsumerConfig
            {
                BootstrapServers = kafkaConsumerConfig.BootstrapServers,
                GroupId = kafkaConsumerConfig.GroupId,
                EnableAutoCommit = kafkaConsumerConfig.EnableAutoCommit,
                EnableAutoOffsetStore = kafkaConsumerConfig.EnableAutoOffsetStore,
                EnablePartitionEof = kafkaConsumerConfig.EnablePartitionEof,

                // Read messages from start if no commit exists.
                AutoOffsetReset = kafkaConsumerConfig.AutoOffsetReset,
                MaxPollIntervalMs = kafkaConsumerConfig.MaxPollIntervalMs,
                SessionTimeoutMs =  kafkaConsumerConfig.SessionTimeoutMs
            });

            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddSingleton<IEventProducer<Order>, OrdersProducer>();
            services.AddSingleton<IEventConsumer<Order>, OrdersConsumer>();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
