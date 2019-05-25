using System;
using System.Collections.Generic;
using System.Text;
using SharedKernel.Models;

namespace PaypalPaymentPlugin.Models
{
    public class PaypalTransaction : BaseEntity
    {
        public int OrderId { get; set; }
        public double PaidAmount { get; set; }
    }
}
