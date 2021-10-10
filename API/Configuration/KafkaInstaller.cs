using API.Domain;
using API.Domain.ConfigurationModels;
using API.Kafka;
using API.Kafka.BackgroundServices;
using API.Kafka.Consumers;
using API.Kafka.Producers;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration
{
    public class KafkaInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureProducers(services, configuration);
            ConfigureSchemaRegistry(services, configuration);
            ConfigureConsumers(services, configuration);
            ConfigureEventHandlers(services);
        }

        private static void ConfigureConsumers(IServiceCollection services, IConfiguration configuration)
        {
            var kafkaConsumerConfig = new ConsumerConfiguration();
            configuration.GetSection("Kafka:Consumers")
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
                SessionTimeoutMs = kafkaConsumerConfig.SessionTimeoutMs
            });
        }

        private static void ConfigureSchemaRegistry(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(new SchemaRegistryConfig
            {
                Url = configuration.GetValue<string>("Kafka:SchemaRegistry:Url")
            });
        }

        private static void ConfigureProducers(IServiceCollection services, IConfiguration configuration)
        {
            var kafkaProducerConfig = new ProducerConfiguration();
            configuration.GetSection("Kafka:Producers")
                .Bind(kafkaProducerConfig);
            services.AddSingleton(new ProducerConfig
            {
                BootstrapServers = kafkaProducerConfig.BootstrapServers,
                Acks = kafkaProducerConfig.Acks,
                MessageSendMaxRetries = kafkaProducerConfig.MessageSendMaxRetries,
                RetryBackoffMs = kafkaProducerConfig.RetryBackoffMs,
                EnableIdempotence = kafkaProducerConfig.EnableIdempotence,
            });
        }

        private static void ConfigureEventHandlers(IServiceCollection services)
        {
            services.AddHostedService<ProcessOrdersBackgroundService>();

            services.AddSingleton<IEventProducer<Order>, OrdersProducer>();
            services.AddSingleton<IEventConsumer<Order>, OrdersConsumer>();
        }
    }
}
