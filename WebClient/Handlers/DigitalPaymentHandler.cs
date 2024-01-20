using MediatorLibrary;
using WebClient.Commands;
using WebClient.Models;

namespace WebClient.Handlers;

//Primary constructor
public class DigitalPaymentHandler(IValidator<DigitalPayment> validator) :
    IReceiver<ValidateDigitalPayment, Response>,
    IReceiver<UpdateDigitalPayment, Response>
{
    
    public Response Handle(ValidateDigitalPayment command)
    {
        var isValid = command.IsValidPayment(validator);
        return new Response() {Result = isValid, StatusCode = 200};
    }
    
    public Response Handle(UpdateDigitalPayment command)
    {
        var payment = Payment.Payments
            .FirstOrDefault(x => x.GetType() == command.Payment.GetType());
        
        payment.Amount = command.Payment.Amount;
        
        return new Response() {Result = command.Payment, StatusCode = 200};
    }
}