using PaypalPaymentPlugin.Models;
using SharedKernel.Data;
using System.Collections.Generic;
using System.Linq;

namespace PaypalPaymentPlugin.Services
{
    //public class PaypalService : IPaypalService
    //{
    //    private readonly IRepository<PaypalTransaction> _repository;
    //    public PaypalService(IRepository<PaypalTransaction> repository)
    //    {
    //        _repository = repository;
    //    }

    //    public List<PaypalTransaction> Test()
    //    {
    //        var p1 = new PaypalTransaction { OrderId = 10, PaidAmount = 500 };
    //        _repository.Insert(p1);

    //        var x = _repository.Table.Where(t => t.PaidAmount > 10).ToList();
    //        return x;
    //    }
    //}
}
