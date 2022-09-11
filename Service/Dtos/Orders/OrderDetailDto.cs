using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Orders
{
    public class OrderDetailDto : OrderDto
    {
        public string StatusString { get; set; }
        public string PaymentMethodString { get; set; }
    }
}
