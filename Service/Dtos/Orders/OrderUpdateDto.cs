using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Orders
{
    public class OrderUpdateDto
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public int? PaymentMethodId { get; set; }
        public DateTimeOffset? PaidDate { get; set; }
        public int StatusId { get; set; }
    }
}
