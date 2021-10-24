using System;
using System.Collections.Generic;
using System.Linq;
using API.Domain;
using Contracts.Requests.Queries;
using Contracts.Responses;

namespace API.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(
            Func<PaginationQuery, Dictionary<string, string>, Uri> getUri,
            PaginationFilter query, IList<T> response,
            Dictionary<string, string> aditionalParams = null
        )
        {
            var nextPage = getUri(new PaginationQuery(query.PageNumber + 1, query.PageSize), aditionalParams)
                .ToString();
            var previousPage = getUri(new PaginationQuery(query.PageNumber - 1, query.PageSize), aditionalParams)
                .ToString();

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = response.Any() ? previousPage : null
            };
        }
    }
}
