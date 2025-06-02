using EventManagingAPI.Data;
using EventManagingAPI.Models;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagingAPI.Repository
{
    public class ManagedEventRepository : IManagedEventRepository
    {
        private readonly EventManagingContext _eventManagingContext;
        
        public ManagedEventRepository(EventManagingContext eventManagingContext)
        {
            _eventManagingContext = eventManagingContext;
        }
        public void Add(ManagedEvent managedEvent)
        {
            _eventManagingContext.ManagedEvents.Add(managedEvent);
        }

        public IEnumerable<ManagedEvent> GetAll()
        {
            return _eventManagingContext.ManagedEvents
                           .Include(e => e.ToDoItems)
                           .Include(e => e.ItemRequirements)
                           .ToList();
        }

        public ManagedEvent GetById(int id)
        {
            return _eventManagingContext.ManagedEvents
                           .Include(e => e.ToDoItems)
                           .Include(e => e.ItemRequirements)
                           .FirstOrDefault(e => e.ManagedEventId == id);
        }

        public IEnumerable<ManagedEvent> GetByVendorId(int vendorId)
        {
            return _eventManagingContext.ManagedEvents
                           .Where(e => e.VendorId == vendorId)
                           .ToList();
        }

        public IEnumerable<ManagedEvent> GetByCustomerId(int customerId)
        {
            return _eventManagingContext.ManagedEvents
                           .Where(e => e.CustomerId == customerId)
                           .ToList();
        }
        public IEnumerable<ManagedEvent> GetByEventRequestId(int eventRequestId)
        {
            return _eventManagingContext.ManagedEvents
                           .Where(e => e.EventRequestId == eventRequestId)
                           .ToList();
        }

        public ManagedEvent GetByVendorAndEventRequest(int vendorId, int eventRequestId)
        {
            return _eventManagingContext.ManagedEvents
                           .FirstOrDefault(e => e.VendorId == vendorId && e.EventRequestId == eventRequestId);
        }

        public void Remove(ManagedEvent managedEvent)
        {
            _eventManagingContext.ManagedEvents.Remove(managedEvent);
        }

        public void Update(ManagedEvent managedEvent)
        {
            _eventManagingContext.ManagedEvents.Update(managedEvent);
        }

        public bool SaveChanges()
        {
            return _eventManagingContext.SaveChanges() > 0;
        }
    }
}
