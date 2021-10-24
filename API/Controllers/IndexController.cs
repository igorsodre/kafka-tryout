using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using API.Helpers;
using API.Services.Interfaces;
using AutoMapper;
using Contracts.Requests;
using Contracts.Requests.Queries;
using Contracts.Responses;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/index")]
    public class IndexController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public IndexController(IMessageRepository messageRepository, IMapper mapper, IUriService uriService)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexRequest request, CancellationToken cancellationToken)
        {
            var result = await _messageRepository.AddMessageAsync(request, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(SuccessResponse.DefaultOkResponse());
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
        {
            var pagedFilter = _mapper.Map<PaginationFilter>(query);

            var result = await _messageRepository.GetMessagesAsync(pagedFilter, cancellationToken);

            return Ok(PaginationHelpers.CreatePaginatedResponse(_uriService.GetMessages, pagedFilter, result.ToList()));
        }
    }
}
