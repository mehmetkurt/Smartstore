using Smartstore.Iyzico.Models;

namespace Smartstore.Iyzico.Services;

public interface IIyzicoPaymentService
{
    Task<InstallmentResponse> GetInstallments(InstallmentQuery query);
}
