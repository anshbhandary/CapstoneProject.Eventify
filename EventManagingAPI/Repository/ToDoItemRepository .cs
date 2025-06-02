using EventManagingAPI.Data;
using EventManagingAPI.Models;
using EventManagingAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagingAPI.Repository
{
    public class ToDoItemRepository  : IToDoItemRepository
    {
        private readonly EventManagingContext _context;

        public ToDoItemRepository(EventManagingContext context)
        {
            _context = context;
        }

        public ToDoItem GetById(int id)
        {
            return _context.ToDoItems
                .Include(t => t.ManagedEvent)
                .FirstOrDefault(t => t.ToDoItemId == id);
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _context.ToDoItems
                .Include(t => t.ManagedEvent)
                .ToList();
        }

        public IEnumerable<ToDoItem> GetByManagedEventId(int managedEventId)
        {
            return _context.ToDoItems
                .Where(t => t.ManagedEventId == managedEventId)
                .ToList();
        }

        public IEnumerable<ToDoItem> GetCompletedItems(int managedEventId)
        {
            return _context.ToDoItems
                .Where(t => t.ManagedEventId == managedEventId && t.IsCompleted)
                .ToList();
        }

        public IEnumerable<ToDoItem> GetPendingItems(int managedEventId)
        {
            return _context.ToDoItems
                .Where(t => t.ManagedEventId == managedEventId && !t.IsCompleted)
                .ToList();
        }

        public void Add(ToDoItem toDoItem)
        {
            _context.ToDoItems.Add(toDoItem);
        }

        public void Update(ToDoItem toDoItem)
        {
            _context.Entry(toDoItem).State = EntityState.Modified;
        }

        public void Remove(ToDoItem toDoItem)
        {
            _context.ToDoItems.Remove(toDoItem);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
