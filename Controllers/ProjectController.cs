using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Kokks.Models;
using Kokks.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kokks.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly ILogger _logger;

        public ProjectController(
            IProjectRepository projectRepository,
            ICollaboratorRepository collaboratorRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _collaboratorRepository = collaboratorRepository;
            _logger = logger.CreateLogger<ProjectController>();
        }

        [HttpGet]
        public async Task<IEnumerable<Project>> GetAll()
        {
            _logger.LogDebug(1, "Getting all projects");
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var projects = _projectRepository.GetAllForUser(user.Id);
            return projects;
        }

        [HttpGet("{id}/collaborators", Name = "GetCollaborators")]
        public IEnumerable<Collaborator> GetCollaborators(long id)
        {
            _logger.LogDebug(2, "Getting collaborators");
            var project = _projectRepository.Find(id);
            var userId = _userManager.GetUserId(HttpContext.User);

            if (_projectRepository.UserHasAccess(project.Id, userId))
            {
                return _collaboratorRepository.FindForProject(project.Id);
            }
            return new List<Collaborator>();
        }

        [HttpGet("{id}", Name = "GetProject")]
        public IActionResult GetById(long id)
        {
            var project = _projectRepository.Find(id);
            var userId = _userManager.GetUserId(HttpContext.User);

            if (_projectRepository.UserHasAccess(project.Id, userId))
            {
                return new ObjectResult(project);
            }
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            _projectRepository.Add(project);
            _collaboratorRepository.Create(userId, project.Id, Permissions.Owner);

            return CreatedAtRoute("GetProject", new { id = project.Id }, project);
        }
    }
}