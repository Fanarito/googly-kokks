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
    public class CollaboratorController : Controller 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly ILogger _logger;

        public CollaboratorController(
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

        [HttpGet("{id}", Name = "GetCollaborator")]
        public IActionResult GetById(long id)
        {
            var collaborator = _collaboratorRepository.Find(id);
            return new ObjectResult(collaborator);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Collaborator item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var collaborator = _collaboratorRepository.Find(item.ProjectID, item.UserID);
            var currentCollaborator = _collaboratorRepository.Find(item.ProjectID, userId);

            if (currentCollaborator == null || currentCollaborator.Permission != Permissions.Owner)
            {
                return new UnauthorizedResult();
            }
            else if (collaborator != null)
            {
                return CreatedAtRoute("GetCollaborator", new { id = collaborator.Id }, collaborator);
            }

            _collaboratorRepository.Add(item);
            var newCollaborator = _collaboratorRepository.Find(item.Id);
            return CreatedAtAction("GetCollaborator", new { id = item.Id }, newCollaborator);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Collaborator item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var currentCollaborator = _collaboratorRepository.Find(item.ProjectID, userId);

            if (currentCollaborator == null || currentCollaborator.Permission != Permissions.Owner)
            {
                return new UnauthorizedResult();
            }

            if (!_projectRepository.UserHasAccess(item.ProjectID, userId))
            {
                return Unauthorized();
            }

            var collaborator = _collaboratorRepository.Find(id);
            if (collaborator == null)
            {
                return NotFound();
            }

            collaborator.Permission = item.Permission;
            _collaboratorRepository.Update(collaborator);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var collaborator = _collaboratorRepository.Find(id);
            if (collaborator == null)
            {
                return NotFound();
            }


            var currentCollaborator = _collaboratorRepository.Find(collaborator.ProjectID, userId);

            // You can only delete if you exist and are trying to delete yourself
            // or when you are the owner of the project
            if (currentCollaborator != null && (currentCollaborator == collaborator || currentCollaborator.Permission == Permissions.Owner))
            {
                _collaboratorRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
