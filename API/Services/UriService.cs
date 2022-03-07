using System;
using System.Collections.Generic;
using API.Services.Interfaces;
using Contracts.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace API.Services;

public class UriService : IUriService
{
    private readonly string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetMessages(PaginationQuery pagination, Dictionary<string, string> aditionalParams = null)
    {
        return GenerateUri(_baseUri + "api/index", pagination, aditionalParams);
    }


    private static Uri GenerateUri(
        string url,
        PaginationQuery pagination,
        Dictionary<string, string> aditionalParams
    )
    {
        var queryParams = aditionalParams is null
            ? new Dictionary<string, string>()
            : new Dictionary<string, string>(aditionalParams);

        queryParams["pageSize"] = pagination.PageSize.ToString();
        queryParams["pageNumber"] = pagination.PageNumber.ToString();

        return new Uri(QueryHelpers.AddQueryString(url, queryParams));
    }
}