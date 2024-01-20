namespace MediatorLibrary;

public interface IReceiver<in TCommand, out TResponse> where TCommand: ICommandBase<TResponse>
{ 
    TResponse Handle(TCommand command);
}