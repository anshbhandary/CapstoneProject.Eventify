using VendorAPI.Models;
using VendorAPI.Models.dto;

namespace VendorAPI.Repository
{
    public interface IServiceRepository
    {
        Task<ServiceSelection> SelectService(ServiceSelectionDto dto);
        Task<ServiceSelection> UpdateService(ServiceSelectionUpdateDto dto);
        Task<bool> DeleteService(int id);
        Task<ServiceSelection> GetServiceById(int id);
        Task<IEnumerable<ServiceSelection>> GetAllServices();
    }
}
