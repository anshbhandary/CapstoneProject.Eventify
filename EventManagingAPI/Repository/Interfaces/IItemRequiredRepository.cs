using EventManagingAPI.Models;

namespace EventManagingAPI.Repository.Interfaces
{
    public interface IItemRequiredRepository
    {
        ItemRequired GetById(int id);
        IEnumerable<ItemRequired> GetAll();
        IEnumerable<ItemRequired> GetByManagedEventId(int managedEventId);
        IEnumerable<ItemRequired> GetAcquiredItems(int managedEventId);
        IEnumerable<ItemRequired> GetPendingItems(int managedEventId);
        void Add(ItemRequired requiredItem);
        void Update(ItemRequired requiredItem);
        void Remove(ItemRequired requiredItem);
        bool SaveChanges();

    }
}
