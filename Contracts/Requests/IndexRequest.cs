using System;
using Contracts.Enums;

namespace Contracts.Requests;

public sealed class IndexRequest
{
    public string Content { get; set; }
    public RequestMessageType Type { get; set; }
    public Guid? RepliedToId { get; set; }
}