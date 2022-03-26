namespace WebApi.Models.Members
{
    public class AdminMember
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
        public int StatusId { get; set; }
        public int ErrorTimes { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public bool IsMaster { get; set; }
        public string HashSalt { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }

        public AdminMemberStatus AdminMemberStatus { get; set; }
    }
}