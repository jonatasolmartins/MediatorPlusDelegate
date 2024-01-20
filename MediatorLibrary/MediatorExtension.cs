using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatorLibrary;

public static class MediatorExtension
{
    //This is the extension method that will be called in Program.cs
    //It extends IServiceCollection
    public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] assemblyTypes)
    {
        var receiversDictionary = new Dictionary<Type, ResponseStateHandler<ICommandBase<IResponseState>, IResponseState>>();
        //Iterate through the types passed as parameter
        foreach (var types in assemblyTypes)
        {
            var assembly = types.Assembly; // the assembly for program.cs is Design.Pattern.Mediator
             
            //Get all the receivers (DigitalPaymentHandler and CashPaymentHandler) and commands from the assembly
            var receivers = GetReceiversTypes(assembly);
            var commands = GetCommandTypes(assembly).ToList();

            //Iterate through the commands and get the receiver that handle the command
            //For example: ValidateDigitalPayment command is handled by DigitalPaymentHandler
            commands.ForEach(cmd =>
            {
                var (command, receiver) = GetFinalReceivers(cmd, receivers, services.BuildServiceProvider());
                if (command == null) return;
                //Add the command type and the receiver to the dictionary
                receiversDictionary.Add(command, receiver);
            });
        }
        
        //Add the mediator class to the container
        services.AddSingleton<IMediator>( x => new Mediator(receiversDictionary));
        return services;
    }
    
    private static IEnumerable<Type> GetCommandTypes(Assembly assembly)
    {
        //Get all the types that implement ICommandBase
        //The !x.IsInterface && !x.IsAbstract is to avoid getting the ICommandBase interface itself we only want the classes that implement it
        var commands = assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandBase<>)));
        return commands;
    }
    private static IEnumerable<Type> GetReceiversTypes(Assembly assembly)
    {
        //Get all the types that implement IReceiver
        var receivers = assembly.GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract && x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReceiver<,>)));
        return receivers;
    }
    
    private static (Type, ResponseStateHandler<ICommandBase<IResponseState>, IResponseState>) GetFinalReceivers(Type command, IEnumerable<Type> receivers, IServiceProvider serviceProvider)
    {
        //Iterate through the receivers and get the one that is a match with the command
        //We are using the GetMethods() to get the methods of the receiver
        var handle = receivers.SingleOrDefault(rc =>
                rc.GetMethods()
                    .Any(x => x
                        .GetParameters()
                        .Any(y => y.ParameterType == command)))?
            .GetMethods()
            .FirstOrDefault(m => m.Name == "Handle" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == command);

        if (handle == null)
            return (null, null);
        
        ParameterExpression input = Expression.Parameter(typeof(ICommandBase<IResponseState>), "commandType");
        // create a expression to convert the command type to its underlying type
        Expression convertedReceiverCommand = Expression.Convert(input, command);
        
        // Get the constructor info of the receiver
        ConstructorInfo constructor = handle.DeclaringType.GetConstructors().First();

        // Get the parameter types of the constructor
        // This is to get the dependencies of the receiver
        // For example: DigitalPaymentHandler has a dependency on IValidator<Payment>
        var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToArray();

        // Use the service provider to resolve the types to actual instances
        var parameters = parameterTypes.Select(serviceProvider.GetService).ToArray();
        
        // Use Activator.CreateInstance to create an instance of the handler with its dependencies
        //DeclaredType is the type (class) where the method handle is declared
        //For example: DigitalPaymentHandler is the DeclaredType of the method handle
        var handlerInstance = Activator.CreateInstance(handle.DeclaringType, parameters);
        
        // create a constant expression for the handler instance
        // ConstantExpression represents an expression that has a constant value
        var handlerInstanceConstant = Expression.Constant(handlerInstance);
        
        // create a call expression for the method
        // which means: handlerInstance.Handle(convertedReceiverCommand);
        var methodCall = Expression.Call(handlerInstanceConstant, handle, convertedReceiverCommand);
        
        // create a lambda expression for the method call
        // which means: (convertedReceiverCommand) => handlerInstance.Handle(convertedReceiverCommand);
        var result =
            Expression.Lambda<ResponseStateHandler<ICommandBase<IResponseState>, IResponseState>>(methodCall, input);
        
        // compile the lambda expression to a delegate
        //Compile() is used to convert the lambda expression to the current instance of the type
        //In this case the type is ResponseStateHandler<ICommandBase<IResponseState>, IResponseState>
        var compiledDelegate = result.Compile();
        
        //The key is the command and the value is the receiver which handle the command
        return (command, compiledDelegate);
    }
}