using MediatorLibrary;
using WebClient.Models;

namespace WebClient.Commands;

public abstract class ValidatePayment<TPayment>(TPayment payment) : ICommandBase<Response>
    where TPayment : Payment
{
    public TPayment Payment { get; set; } = payment;

    public bool IsValidPayment(IValidator<TPayment> validator)
    {
        return validator.Validate(Payment);
    }
}

public class ValidateCashPayment(CashPayment payment) : ValidatePayment<CashPayment>(payment);

public class ValidateDigitalPayment(DigitalPayment payment) : ValidatePayment<DigitalPayment>(payment);