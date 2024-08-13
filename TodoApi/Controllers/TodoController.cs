using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.TODO;
using TodoApi.Repository;
using TodoApi.Service.Identity;

namespace TodoApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoRepository _repository;
        private readonly PrincipalService _principalService;

        public TodoController(
            TodoRepository todoRepository,
            PrincipalService userService
        )
        {
            _repository = todoRepository;
            _principalService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userId = _principalService.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var todos = _repository.FindByUserId(userId);
            return Ok(todos);
        }

        [HttpGet("{id:int}")]
        public ActionResult Details(int id)
        {
            var userId = _principalService.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var todo = _repository.FetchById(id);
            if (todo is null)
                return NotFound();

            if (todo.UserId != userId)
                return NotFound();

            return Ok(todo);
        }

        [HttpPost]
        public ActionResult Create([FromBody] TodoCreateRequest request)
        {
            if (!request.Validate().IsValid)
                return BadRequest();

            var userId = _principalService.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var todo = new Todo()
            {
                Title = request.Title,
                IsDone = request.IsDone,
                UserId = userId
            };
            if (!todo.Validate().IsValid)
                return BadRequest();

            _repository.Add(todo);
            return Created();
        }

        [HttpPatch("{id:int}")]
        public ActionResult Edit(int id, [FromBody] TodoRequest todoRequest)
        {
            if (!todoRequest.Validate().IsValid)
                return BadRequest();

            var userId = _principalService.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var todo = _repository.FetchById(id);
            if (todo is null)
                return NotFound();

            todo.Title = todoRequest.Title;
            todo.IsDone = todoRequest.IsDone;

            _repository.Update(todo);
            return Ok(todo);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var userId = _principalService.GetUserId(User);
            if (userId is null)
                return Unauthorized();

            var todo = _repository.FetchById(id);
            if (todo is null)
                return NotFound();
            if (todo.UserId != userId)
                return Unauthorized();

            _repository.Remove(todo);
            return Ok(todo);
        }
    }
}