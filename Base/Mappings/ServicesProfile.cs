using AutoMapper;
using WebApi.Dtos.Members;
using WebApi.Models.Members;

namespace WebApi.Base.Mappings
{
    public class ServicesProfile : Profile
    {
        public ServicesProfile()
        {
            // AdminMember
            CreateMap<AdminMember, AdminMemberInfoModel>();
        }
    }
}