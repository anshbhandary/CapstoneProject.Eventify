using AdminAPI.Data;
using AdminAPI.Models;
using AdminAPI.Repository.Interfaces;

namespace AdminAPI.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AdminDbContext _dbContext;

        public AdminRepository(AdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add(Admin admin)
        {
            _dbContext.admins.Add(admin);
        }

        public void Delete(Admin admin)
        {
            _dbContext.admins.Remove(admin);
        }

        public void Update(Admin admin)
        {
            _dbContext.admins.Update(admin);
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
