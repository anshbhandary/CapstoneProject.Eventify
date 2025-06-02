using EventManagingAPI.Data;
using EventManagingAPI.Models;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagingAPI.Repository
{
    public class ItemRequiredRepository  : IItemRequiredRepository
    {
        private readonly EventManagingContext _context;

        public ItemRequiredRepository(EventManagingContext context)
        {
            _context = context;
        }

        public ItemRequired GetById(int id)
        {
            return _context.ItemRequirements
                .Include(r => r.ManagedEvent)
                .FirstOrDefault(r => r.RequiredItemId == id);
        }

        public IEnumerable<ItemRequired> GetAll()
        {
            return _context.ItemRequirements
                .Include(r => r.ManagedEvent)
                .ToList();
        }

        public IEnumerable<ItemRequired> GetByManagedEventId(int managedEventId)
        {
            return _context.ItemRequirements
                .Where(r => r.ManagedEventId == managedEventId)
                .ToList();
        }

        public IEnumerable<ItemRequired> GetAcquiredItems(int managedEventId)
        {
            return _context.ItemRequirements
                .Where(r => r.ManagedEventId == managedEventId && r.IsAcquired)
                .ToList();
        }

        public IEnumerable<ItemRequired> GetPendingItems(int managedEventId)
        {
            return _context.ItemRequirements
                .Where(r => r.ManagedEventId == managedEventId && !r.IsAcquired)
                .ToList();
        }

        public void Add(ItemRequired requiredItem)
        {
            _context.ItemRequirements.Add(requiredItem);
        }

        public void Update(ItemRequired requiredItem)
        {
            _context.Entry(requiredItem).State = EntityState.Modified;
        }

        public void Remove(ItemRequired requiredItem)
        {
            _context.ItemRequirements.Remove(requiredItem);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
