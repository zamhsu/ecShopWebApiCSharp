using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Members
{
    public class AdminMemberUpdateDto
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public int StatusId { get; set; }
        public int ErrorTimes { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public bool IsMaster { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}
