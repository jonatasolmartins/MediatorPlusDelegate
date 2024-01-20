namespace MediatorLibrary;

public interface IMediator
{ 
    public TResponse Send<TResponse>(ICommandBase<TResponse> command);
    public Task<TResponse> SendAsync<TResponse>(ICommandBase<TResponse> command);
}