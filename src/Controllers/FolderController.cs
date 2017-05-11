using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Kokks.Models;
using Kokks.Services;

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
        private readonly PermissionServices _permissionServices;

        public FolderController(
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IFolderRepository folderRepository,
            ICollaboratorRepository collaboratorRepository,
            PermissionServices permissionServices
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _logger = logger.CreateLogger<ProjectController>();
            _folderRepository = folderRepository;
            _collaboratorRepository = collaboratorRepository;
            _permissionServices = permissionServices;
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
            if (_permissionServices.HasWriteAccess(item.ProjectID, userId))
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
            if (_permissionServices.HasWriteAccess(item.ProjectID, userId))
            {
                return Unauthorized();
            }

            var folder = _folderRepository.Find(id);
            if (folder == null)
            {
                return NotFound();
            }

            folder.Name = item.Name;

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

            if (_permissionServices.HasWriteAccess(folder.ProjectID, userId))
            {
                _folderRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
