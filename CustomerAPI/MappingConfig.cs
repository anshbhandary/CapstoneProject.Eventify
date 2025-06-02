using AutoMapper;
using CustomerAPI.Modals;
using CustomerAPI.Modals.Dto;

namespace CustomerAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CustomerDto, Customer>();
                config.CreateMap<Customer, CustomerDto>();
                config.CreateMap<EventRequirementDto, EventRequirements>();
                config.CreateMap<EventRequirements,EventRequirementDto>();
                config.CreateMap<GuestInfo,GuestInfoDto>();
                config.CreateMap<GuestInfoDto, GuestInfo>();
                config.CreateMap<ServiceRequest, ServiceRequestDto>();
                config.CreateMap<ServiceRequestDto,ServiceRequest>();
            });
            return mappingConfig;
        }

    }
}
