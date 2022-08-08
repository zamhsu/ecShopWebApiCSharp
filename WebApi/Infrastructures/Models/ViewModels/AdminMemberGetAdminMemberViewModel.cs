using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Members;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class AdminMemberGetAdminMemberViewModel
    {
        public List<AdminMemberDisplayDto> AdminMemberDisplays { get; set; } = null!;

        public Pagination Pagination { get; set; } = null!;
    }
}