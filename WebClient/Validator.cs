using WebClient.Models;

namespace WebClient;

public interface IValidator<in T>
{
    bool Validate(T item);
}
 
public class PaymentValidator<TPayment> : IValidator<TPayment> where TPayment : Payment
{
    public bool Validate(TPayment item)
    {
        return item.Amount > 0;
    }
}