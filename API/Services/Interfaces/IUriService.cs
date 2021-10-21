using System;
using System.Collections.Generic;
using Contracts.Requests.Queries;

namespace API.Services.Interfaces
{
    public interface IUriService
    {
        Uri GetMessages(PaginationQuery pagination, Dictionary<string, string> aditionalParams = null);
    }
}
