namespace Service.Dtos.Members
{
    public class AdminMemberCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
        public string HashSalt { get; set; }
    }
}
