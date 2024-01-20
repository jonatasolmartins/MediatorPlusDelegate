namespace MediatorLibrary;

public interface IResponseState
{
    public object Result { get; set; }
    public int StatusCode { get; set; }
}