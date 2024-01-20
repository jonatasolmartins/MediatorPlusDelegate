
namespace MediatorLibrary;

public delegate TResponseState ResponseStateHandler<in TCommand, out TResponseState>(TCommand command)
    where TCommand: ICommandBase<TResponseState>;


public class Mediator(IReadOnlyDictionary<Type, ResponseStateHandler<ICommandBase<IResponseState>, IResponseState>> serviceProvider)
    : IMediator
{
    private ValueTask<IResponseState> GetResponse(ICommandBase<IResponseState> command)
    {
        //Get the method from the dictionary of methods
       var method = serviceProvider[command.GetType()];
       //Invoke the method delegate
       var response = method(command);
       
       return ValueTask.FromResult(response);
    }
    
    public async Task<TResponse> SendAsync<TResponse>(ICommandBase<TResponse> command)
    { 
        return (TResponse) await GetResponse((ICommandBase<IResponseState>)command);
    }
    
    public TResponse Send<TResponse>(ICommandBase<TResponse> command)
    { 
        return (TResponse) GetResponse((ICommandBase<IResponseState>)command).GetAwaiter().GetResult();
    }
}

