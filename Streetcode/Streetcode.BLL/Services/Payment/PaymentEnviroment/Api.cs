namespace Streetcode.BLL.Services.Payment.PaymentEnviroment;

public class Api
{
    public string Production { get; set; } = string.Empty;
    public Merchant Merchant { get; set; } = new();
}
