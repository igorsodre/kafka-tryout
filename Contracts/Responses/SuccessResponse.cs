namespace Contracts.Responses;

public class SuccessResponse<T>
{
    public SuccessResponse() { }

    public SuccessResponse(T response)
    {
        Data = response;
    }

    public T Data { get; set; }
}

public class SuccessResponse : SuccessResponse<string>
{
    public static SuccessResponse<string> DefaultOkResponse()
    {
        return new SuccessResponse<string>("OK");
    }
}