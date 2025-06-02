using AdminAPI.Models;
using AdminAPI.Models.Dto;
using AutoMapper;

namespace AdminAPI
{
    public class MappingConfig
    {
        public static  MapperConfiguration RegisterMaps()
        {
            var mapConfig = new MapperConfiguration(
                config =>
                {
                    config.CreateMap<Admin, AdminDto>();
                    config.CreateMap<AdminDto, Admin>();

                    config.CreateMap<Approval,ApprovalResponseDto>();
                    config.CreateMap<ApprovalDto, Approval>();
                    config.CreateMap<Approval, ApprovalDto>();
                    config.CreateMap<ApprovalUpdateDto, Approval>();
                    config.CreateMap<ApprovalDto, ApprovalResponseDto>();

                }
                
                );
            return mapConfig;
        }
    }
}
