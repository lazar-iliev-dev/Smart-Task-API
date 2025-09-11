using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync() 
        => _repository.GetAllAsync();

    public Task<TaskItem?> GetTaskByIdAsync(Guid id) 
        => _repository.GetByIdAsync(id);

    public Task<TaskItem> CreateTaskAsync(TaskItem task) 
        => _repository.AddAsync(task);

    public Task<TaskItem?> UpdateTaskAsync(TaskItem task) 
        => _repository.UpdateAsync(task);

    public Task<TaskItem?> DeleteTaskAsync(Guid id) 
        => _repository.DeleteAsync(id);
    }
}
