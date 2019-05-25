using SharedKernel.Models;
using System.Collections.Generic;

namespace Core.Models
{
    public class Order : BaseEntity
    {
        public string Buyer { get; set; }
        public double Price { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
    }
}
