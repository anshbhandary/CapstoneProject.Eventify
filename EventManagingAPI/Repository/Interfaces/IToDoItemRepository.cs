using EventManagingAPI.Models;

namespace EventManagingAPI.Repository.Interfaces
{
    public interface IToDoItemRepository
    {
        ToDoItem GetById(int id);
        IEnumerable<ToDoItem> GetAll();
        IEnumerable<ToDoItem> GetByManagedEventId(int managedEventId);
        IEnumerable<ToDoItem> GetCompletedItems(int managedEventId);
        IEnumerable<ToDoItem> GetPendingItems(int managedEventId);
        void Add(ToDoItem toDoItem);
        void Update(ToDoItem toDoItem);
        void Remove(ToDoItem toDoItem);
        bool SaveChanges();

    }
}
