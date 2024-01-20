using MediatorLibrary;

namespace WebClient;

public class Response : IResponseState
{
    public object Result { get; set; }
    public int StatusCode { get; set; }
}