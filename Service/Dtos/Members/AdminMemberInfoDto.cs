namespace Service.Dtos.Members
{
    public class AdminMemberInfoDto
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}