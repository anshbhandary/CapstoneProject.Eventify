using Microsoft.EntityFrameworkCore;
using VendorAPI.Data;
using VendorAPI.Models;
using VendorAPI.Models.dto;

namespace VendorAPI.Repository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly AppDbContext _context;

        public VendorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vendor> RegisterVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }

        public async Task<Vendor> GetVendorByEmail(string email)
        {
            return await _context.Vendors.FirstOrDefaultAsync(v => v.Email == email);
        }

        public async Task<Vendor> GetVendorById(int id)
        {
            return await _context.Vendors.Include(v => v.Packages).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VendorPackage>> GetPackagesByVendorId(int vendorId)
        {
            return await _context.VendorPackages.Where(p => p.VendorId == vendorId).ToListAsync();
        }

        public async Task AddPackage(VendorPackage package)
        {
            _context.VendorPackages.Add(package);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePackage(int packageId, int vendorId)
        {
            var package = await _context.VendorPackages.FirstOrDefaultAsync(p => p.Id == packageId && p.VendorId == vendorId);

            if (package == null)
            {
                return false;
            }

            _context.VendorPackages.Remove(package);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePackage(int packageId, int vendorId, PackageUpdateDto dto)
        {
            var package = await _context.VendorPackages
                .FirstOrDefaultAsync(p => p.Id == packageId && p.VendorId == vendorId);

            if (package == null)
                return false;

            // Update fields
            package.PackageName = dto.Name;
            package.Description = dto.Description;
            package.Price = dto.Price;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
