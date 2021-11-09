using System.Collections.Generic;

namespace WebApi.Models.Members
{
    public class AdminMemberStatus
    {
        public AdminMemberStatus()
        {
            AdminMember = new HashSet<AdminMember>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AdminMember> AdminMember { get; set; }
    }
}