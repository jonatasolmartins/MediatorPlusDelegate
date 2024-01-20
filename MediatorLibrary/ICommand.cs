namespace MediatorLibrary;

public interface ICommandBase<out T> { }
public interface ICommand<out TResponse>  : ICommandBase<TResponse>{ }