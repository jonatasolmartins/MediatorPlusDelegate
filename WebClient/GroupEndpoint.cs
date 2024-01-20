using MediatorLibrary;
using WebClient.Commands;
using WebClient.Models;

namespace WebClient;

public static class GroupEndpoint
{
    public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/Payment", GetPayments).WithName("GetPayments").WithOpenApi();
        
        group.MapPost("/Payment/Validate", ValidateDigitalPayment).WithName("ValidatePayment").WithOpenApi();
        
        group.MapPut("/Payment/Update", UpdateDigitalPayment).WithName("UpdatePayment").WithOpenApi();
        
        return group;
    }
    
    private static async Task<Response> ValidateDigitalPayment(ValidateDigitalPayment payment, IMediator mediator)
    {
        return await mediator.SendAsync(payment);
    }
    
    private static async Task<Response> UpdateDigitalPayment(DigitalPayment payment, IMediator mediator)
    {
        return await mediator.SendAsync(new UpdateDigitalPayment(payment));
    }

    private static IResult GetPayments()
    {
        return Results.Ok(Payment.Payments);
    }
}