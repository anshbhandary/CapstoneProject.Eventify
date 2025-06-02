using VendorAPI.Models;
using VendorAPI.Models.dto;

namespace VendorAPI.Repository
{
    public interface IVendorRepository
    {
        Task<Vendor> RegisterVendor(Vendor vendor);
        Task<Vendor> GetVendorByEmail(string email);
        Task<Vendor> GetVendorById(int id);
        Task<IEnumerable<VendorPackage>> GetPackagesByVendorId(int vendorId);
        Task AddPackage(VendorPackage package);

        Task<bool> DeletePackage(int packageId, int vendorId);

        Task<bool> UpdatePackage(int packageId, int vendorId, PackageUpdateDto dto);

    }
}
