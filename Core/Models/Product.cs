using System.Collections.Generic;
using SharedKernel.Models;

namespace Core.Models
{ 
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        public IList<Order> Orders { get; set; }

    }
}
