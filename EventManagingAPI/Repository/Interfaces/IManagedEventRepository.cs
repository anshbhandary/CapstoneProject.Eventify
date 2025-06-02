using EventManagingAPI.Models;

namespace EventManagingAPI.Repository.Interfaces
{
    public interface IManagedEventRepository
    {
        ManagedEvent GetById(int id);
        IEnumerable<ManagedEvent> GetAll();
        IEnumerable<ManagedEvent> GetByVendorId(int vendorId);
        IEnumerable<ManagedEvent> GetByCustomerId(int customerId);
        IEnumerable<ManagedEvent> GetByEventRequestId(int eventRequestId);
        ManagedEvent GetByVendorAndEventRequest(int vendorId, int eventRequestId);
        void Add(ManagedEvent managedEvent);
        void Update(ManagedEvent managedEvent);
        void Remove(ManagedEvent managedEvent);
        bool SaveChanges();

    }
}
