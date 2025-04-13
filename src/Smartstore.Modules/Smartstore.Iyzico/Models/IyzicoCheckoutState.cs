using Smartstore.ComponentModel;

namespace Smartstore.Iyzico.Models;

[Serializable]
public class IyzicoCheckoutState : ObservableObject
{
    public int Installment
    {
        get => GetProperty<int>();
        set => SetProperty(value);
    }

    public string CreditCardNumberMask
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string CardType
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }

    public string BrandName
    {
        get => GetProperty<string>();
        set => SetProperty(value);
    }
}
