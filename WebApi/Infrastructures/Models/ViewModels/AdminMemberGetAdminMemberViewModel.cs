using Common.Dtos;
using WebApi.Infrastructures.Models.Dtos.Members;

namespace WebApi.Infrastructures.Models.ViewModels
{
    public class AdminMemberGetAdminMemberViewModel
    {
        public List<AdminMemberDisplayDto> AdminMemberDisplays { get; set; }

        public Pagination Pagination { get; set; }
    }
}