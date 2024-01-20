using System.Text.Json.Serialization;

namespace WebClient.Models;
public class Payment
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
    
    public static List<Payment> Payments = new()
    {
        new CashPayment() {Amount = 1000m},
        new CreditCardPayment() {Amount = 5000m},
        new PaypalPayment() {Amount = 200m},
        new DigitalPayment() {Amount = 100m}
    };
}

public class CashPayment : Payment { }
public class DigitalPayment : Payment { }
public class PaypalPayment : DigitalPayment { }
public class CreditCardPayment : DigitalPayment { }
