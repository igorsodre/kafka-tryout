using API.Domain;
using Contracts.Requests.Queries;
using AutoMapper;

namespace API.MappingProfiles;

public class RequestToDomainProfile : Profile
{
    public RequestToDomainProfile()
    {
        CreateMap<PaginationQuery, PaginationFilter>();
    }
}