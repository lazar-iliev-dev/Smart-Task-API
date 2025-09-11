using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.TaskItems.FindAsync(id);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await _context.TaskItems.AddAsync(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        var existing = await _context.TaskItems.FindAsync(task.TaskId);
        if (existing == null) return null;

        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.DueDate = task.DueDate;
        existing.Priority = task.Priority;
        existing.Status = task.Status;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<TaskItem?> DeleteAsync(Guid id)
    {
        var task = await _context.TaskItems.FindAsync(id);
        if (task == null) return null;

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
        return task;
    }

    }
}
