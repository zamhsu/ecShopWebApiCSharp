namespace WebApi.Infrastructures.Models.Paramaters
{
    public class CreateAdminMemberParameter
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
    }
}