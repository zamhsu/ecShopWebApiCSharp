using WebApi.Dtos.Members;

namespace WebApi.Dtos.ViewModel
{
    public class AdminMemberGetAdminMemberViewModel
    {
        public List<AdminMemberDisplayModel> AdminMemberDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}