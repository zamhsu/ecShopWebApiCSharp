namespace WebApi.Infrastructures.Models.Dtos.Members
{
    public class AdminMemberDisplayDto
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int StatusId { get; set; }
        public string StatusString { get; set; }
        public int ErrorTimes { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public bool IsMaster { get; set; }
    }
}