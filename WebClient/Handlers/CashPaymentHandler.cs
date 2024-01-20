using MediatorLibrary;
using WebClient.Commands;
using WebClient.Models;

namespace WebClient.Handlers;

public class CashPaymentHandler(IValidator<Payment> validator) : IReceiver<ValidateCashPayment, Response>
{
    public Response Handle(ValidateCashPayment command)
    {
        var isValid = command.IsValidPayment(validator);
        return new Response() {Result = isValid, StatusCode = 200};
    }
}