using AdminAPI.Models;

namespace AdminAPI.Repository.Interfaces
{
    public interface IAprovalRepository
    {
        void Add(Approval approval);
        void Update(Approval approval);
        void Delete(Approval approval);
        bool SaveChanges();

        Approval GetStatus(int id);
        IEnumerable<Approval> GetAll();

    }
}
