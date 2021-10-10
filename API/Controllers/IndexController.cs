using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Services.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/")]
    public class IndexController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public IndexController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IndexRequest request, CancellationToken cancellationToken)
        {
            var result = await _messageRepository.AddMessageAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(SuccessResponse.DefaultOkResponse());
        }

        [HttpGet]
        public async Task<IActionResult> Index(uint? page, uint? numOfRows, CancellationToken cancellationToken)
        {
            var result =
                await _messageRepository.GetMessagesAsync(page, numOfRows, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.ErrorMessages));
            }

            return Ok(new SuccessResponse<IEnumerable<Message>>(result.Data));
        }
    }
}
