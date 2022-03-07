using System.Collections.Generic;
using System.Linq;

namespace Contracts.Responses;

public class ErrorResponse
{
    public ErrorResponse() { }

    public ErrorResponse(IEnumerable<string> errorMessages)
    {
        Errors = new List<ErrorModel>(errorMessages.Select(e => new ErrorModel { Message = e }));
    }

    public IEnumerable<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
}