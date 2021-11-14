namespace WebApi.Dtos.Members
{
    public class AdminMemberInfoModel
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public int StatusId { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}