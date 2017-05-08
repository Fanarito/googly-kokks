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
    public class FolderController : Controller 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger _logger;
        private readonly IFolderRepository _folderRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;

        public FolderController(
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IFolderRepository folderRepository,
            ICollaboratorRepository collaboratorRepository
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _logger = logger.CreateLogger<ProjectController>();
            _folderRepository = folderRepository;
            _collaboratorRepository = collaboratorRepository;
        }

        [HttpGet]
        public IEnumerable<Folder> GetAll()
        {
            // ?
            return _folderRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetFolder")]
        public IActionResult GetById(long id)
        {
            var folder = _folderRepository.Find(id);
            return new ObjectResult(folder);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Folder item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var currentCollaborator = _collaboratorRepository.Find(item.ProjectID, userId);

            if (currentCollaborator == null || (currentCollaborator.Permission != Permissions.Owner
               && currentCollaborator.Permission != Permissions.ReadWrite))
            {
                return new UnauthorizedResult();
            }

            _folderRepository.Add(item);
            return CreatedAtAction("GetFolder", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Folder item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var currentCollaborator = _collaboratorRepository.Find(item.ProjectID, userId);

            if (currentCollaborator == null || (currentCollaborator.Permission != Permissions.Owner
               && currentCollaborator.Permission != Permissions.ReadWrite))
            {
                return new UnauthorizedResult();
            }

            if (!_projectRepository.UserHasAccess(item.ProjectID, userId))
            {
                return Unauthorized();
            }

            var folder = _folderRepository.Find(id);
            if (folder == null)
            {
                return NotFound();
            }

            _folderRepository.Update(folder);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var folder = _folderRepository.Find(id);
            if (folder == null)
            {
                return NotFound();
            }


            var currentCollaborator = _collaboratorRepository.Find(folder.ProjectID, userId);

            // You can only delete if you are a collaborator 
            // and own the project or have readWrite permission
            if (currentCollaborator != null || (currentCollaborator.Permission == Permissions.Owner
                || currentCollaborator.Permission == Permissions.ReadWrite))
            {
                _folderRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
