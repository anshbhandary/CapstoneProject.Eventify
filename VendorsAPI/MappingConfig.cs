using AutoMapper;
using VendorAPI.Models;
using VendorAPI.Models.dto;
using VendorsAPI.Models.dto;

namespace VendorAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Vendor, VendorResponseDto>();
                config.CreateMap<PackageCreateDto, VendorPackage>();
                config.CreateMap<VendorPackage, PackageResponseDto>();
                config.CreateMap<PackageUpdateDto,VendorPackage>();



                config.CreateMap<ServiceSelectionDto, ServiceSelection>();
                config.CreateMap<ServiceSelectionUpdateDto, ServiceSelection>();
                config.CreateMap<ServiceSelection, ServiceSelectionResponseDto>();



                config.CreateMap<QuotationDto, Quotation>();
                config.CreateMap<AvailabilitySlotDto, AvailabilitySlot>();
                config.CreateMap<BookingDto, Booking>();
            });
            return mappingConfig;
            
        }

    }
}
