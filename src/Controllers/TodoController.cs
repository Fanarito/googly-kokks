using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kokks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Kokks.Handlers;
using WebSocketManager.Common;

namespace Kokks.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private TodoItemHandler _todoHandler;

        public TodoController(
            ITodoRepository todoRepository,
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            TodoItemHandler todoHandler
        )
        {
            _todoRepository = todoRepository;
            _projectRepository = projectRepository;
            _userManager = userManager;
            _todoHandler = todoHandler;
        }

        [HttpGet]
        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            var userId = await _userManager.GetUserAsync(HttpContext.User);
            return _todoRepository.GetAllForUser(userId.Id);
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var item = _todoRepository.Find(id);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (item == null)
            {
                return NotFound();
            }

            if (_projectRepository.UserHasAccess(item.ProjectID, userId))
            {
                return new ObjectResult(item);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            if (_projectRepository.UserHasAccess(item.ProjectID, userId))
            {
                _todoRepository.Add(item);
                // Broadcast the new todo
                _todoHandler.AddTodo(item.Id, item.Name, item.ProjectID);
                return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _todoRepository.Find(id);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (todo == null)
            {
                return NotFound();
            }
            else if (_projectRepository.UserHasAccess(item.ProjectID, userId))
            {
                todo.IsComplete = item.IsComplete;
                todo.Name = item.Name;

                _todoRepository.Update(todo);
                return new NoContentResult();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _todoRepository.Find(id);
            var userId = _userManager.GetUserId(HttpContext.User);
            if (todo == null)
            {
                return NotFound();
            }
            else if (_projectRepository.UserHasAccess(todo.ProjectID, userId))
            {
                // Broadcast todo remove
                _todoHandler.DeleteTodo(todo.Id, todo.ProjectID);
                _todoRepository.Remove(id);
                return new NoContentResult();
            }
            else {
                return Unauthorized();
            }

        }
    }
}
