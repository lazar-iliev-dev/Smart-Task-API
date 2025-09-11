using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task<TaskItem?> UpdateAsync(TaskItem task);
        Task<TaskItem?> DeleteAsync(Guid id);
    }
}
