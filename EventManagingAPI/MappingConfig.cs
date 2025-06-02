using AutoMapper;
using EventManagingAPI.Models;
using EventManagingAPI.Models.Dto;

namespace EventManagingAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(
                config =>
                {
                    // ManagedEvent mappings
                    config.CreateMap<ManagedEvent, ManagedEventResponseDTO>();
                    config.CreateMap<ManagedEventDTO, ManagedEvent>();

                    // ToDoItem mappings
                    config.CreateMap<ToDoItem, ToDoItemResponseDTO>();
                    config.CreateMap<ToDoItemDTO, ToDoItem>();
                    config.CreateMap<ToDoItemUpdateDTO, ToDoItem>();

                    // RequiredItem mappings (consistent naming)
                    config.CreateMap<ItemRequired, RequiredItemResponseDTO>();
                    config.CreateMap<RequiredItemDTO, ItemRequired>();
                    config.CreateMap<RequiredItemUpdateDTO, ItemRequired>();
                });
            return mappingConfig;
        }
    }
}