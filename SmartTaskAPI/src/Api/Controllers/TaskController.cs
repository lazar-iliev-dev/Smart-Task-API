using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskRepository _repo;

        public TaskController(AppDbContext context, ILogger<TaskController> logger, ITaskRepository repo)
        {
            _context = context;
            _logger = logger;
            _repo = repo;
        }

        // GET api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            try
            {
                var tasks = await _context.TaskItems.ToListAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when retrieving the tasks.");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid id)
        {
            try
            {
                var taskItem = await _context.TaskItems.FindAsync(id);
                if (taskItem != null)
                {
                    _logger.LogInformation("Task with ID {Id} was successfully retrieved.", id);
                    return Ok(taskItem);
                }
                else
                {
                    _logger.LogWarning("Task with ID {TaskId} was not found.", id);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when retrieving the task with ID {TaskId}", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST api/tasks
        [HttpPost]
public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskDto dto)
{
    var entity = new TaskItem
    {
        TaskId = Guid.NewGuid(),
        Title = dto.Title,
        Description = dto.Description,
        DueDate = dto.DueDate,
        Status = dto.Status,
        Priority = dto.Priority,
        CreatedAt = DateTime.UtcNow
    };

    _context.TaskItems.Add(entity);

    try
    {
        await _context.SaveChangesAsync();
        _logger.LogInformation("Task '{Title}' successfully added.", entity.Title);
        return CreatedAtAction(nameof(GetTask), new { id = entity.TaskId }, entity);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating the task '{Title}'", entity.Title);
        return BadRequest();
    }
}


        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto dto)
        {
            if (dto == null) return BadRequest("Invalid request body");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            // Strings
            existing.Title = dto.Title ?? existing.Title;
            existing.Description = dto.Description ?? existing.Description;
            existing.DueDate = dto.DueDate ?? existing.DueDate;

            // Priority (int -> Enum)
            if (dto.Priority.HasValue)
            {
                if (Enum.IsDefined(typeof(Priority), dto.Priority.Value))
                {
                    existing.Priority = (Priority)dto.Priority.Value;
                }
                else
                {
                    return BadRequest($"Invalid Priority value: {dto.Priority}");
                }
            }

            // Status (int -> Enum)
            if (dto.Status.HasValue)
            {
                if (Enum.IsDefined(typeof(Status), dto.Status.Value))
                {
                    existing.Status = (Status)dto.Status.Value;
                }
                else
                {
                    return BadRequest($"Invalid Status value: {dto.Status}");
                }
            }

            await _repo.UpdateAsync(existing);
            return NoContent();






            // if (id != task.TaskId)
            // {
            //     _logger.LogWarning("Mismatch: route ID {TaskId} does not match task ID {TaskItemId}.", id, task.TaskId);
            //     return BadRequest();
            // }

            // _context.Entry(task).State = EntityState.Modified;

            // try
            // {
            //     await _context.SaveChangesAsync();
            //     _logger.LogInformation("Task with ID {TaskId} successfully updated.", id);
            // }
            // catch (DbUpdateConcurrencyException)
            // {
            //     if (!_context.TaskItems.Any(t => t.TaskId == id))
            //     {
            //         _logger.LogWarning("Task with ID {TaskId} not found during update.", id);
            //         return NotFound();
            //     }
            //     throw;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, "Error updating task with ID {TaskId}.", id);
            //     return StatusCode(500, "Internal server error");
            // }

            // return NoContent();
        }

        // DELETE api/tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found for deletion.", id);
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Task with ID {TaskId} successfully deleted.", id);

            return NoContent();
        }
    }
}
