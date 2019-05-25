using System.Collections.Generic;
using PaypalPaymentPlugin.Models;

namespace PaypalPaymentPlugin.Services
{
    public interface IPaypalService
    {
        List<PaypalTransaction> Test();
    }
}