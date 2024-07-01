namespace Streetcode.BLL.Services.Payment.PaymentEnviroment;

public class PaymentEnvirovmentVariables
{
    public string Token { get; set; } = string.Empty;
    public string XToken { get; set; } = string.Empty;
    public Api Api { get; set; } = new();
}