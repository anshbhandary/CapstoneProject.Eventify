using AdminAPI.Data;
using AdminAPI.Models;
using AdminAPI.Repository.Interfaces;
using AutoMapper;
using EventManagingAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace AdminAPI.Repository
{
    public class AprovalRepository : IAprovalRepository
    {
        private readonly AdminDbContext _adminDbContext;

        public AprovalRepository(AdminDbContext adminDbContext)
        {
            _adminDbContext = adminDbContext;
        }

        public void Add(Approval approval)
        {
            _adminDbContext.approvals.Add(approval);
        }

        public void Update(Approval approval)
        {
            _adminDbContext.approvals.Update(approval);
        }

        public void Delete(Approval approval)
        {
            _adminDbContext.approvals.Remove(approval);
        }
        public bool SaveChanges()
        {
            return _adminDbContext.SaveChanges() > 0;
        }

        public Approval GetStatus(int id)
        {
            return _adminDbContext.approvals
                .FirstOrDefault(r => r.ApprovalRequestId == id);
        }

        public IEnumerable<Approval> GetAll()
        {
            return _adminDbContext.approvals.ToList();
        }

    }
}
