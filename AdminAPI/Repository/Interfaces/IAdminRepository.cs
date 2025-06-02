using AdminAPI.Models;

namespace AdminAPI.Repository.Interfaces
{
    public interface IAdminRepository
    {
        
        void Add(Admin admin);
        void Delete(Admin admin);
        void Update(Admin admin);
        bool SaveChanges();
    }
}
