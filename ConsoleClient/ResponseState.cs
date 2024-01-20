using MediatorLibrary;

namespace ConsoleClient;
public class ResponseState : IResponseState
{
    public object Result { get; set; }
    public int StatusCode { get; set; }
}