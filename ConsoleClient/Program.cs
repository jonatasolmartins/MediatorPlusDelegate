using MediatorLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClient;

public static class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddMediator(typeof(Program));
        
        var serviceProvider = services.BuildServiceProvider();
        IMediator mediator = serviceProvider.GetService<IMediator>();
        
        var response =  mediator.Send(new CreateUser());
        Console.WriteLine($"User created, Id: {response.Result}, Code: {response.StatusCode}");
        
        response =  mediator.Send(new UpdateUser() {User = new User() {Id = 1, Name = "My name"}});
        Console.WriteLine($"{response.Result}, Code: {response.StatusCode}");
    }
}





