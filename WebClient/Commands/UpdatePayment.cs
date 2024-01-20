using MediatorLibrary;
using WebClient.Models;

namespace WebClient.Commands;
public abstract class UpdatePayment<TPayment>(TPayment payment) : ICommandBase<Response>
    where TPayment : Payment
{
    public TPayment Payment { get; set; } = payment;
}

public class UpdateDigitalPayment(DigitalPayment payment) : UpdatePayment<DigitalPayment>(payment);