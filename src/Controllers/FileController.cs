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
    public class FileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger _logger;
        private readonly IFolderRepository _folderRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly IFileRepository _fileRepository;
        private readonly PermissionServices _permissionServices;

        public FileController(
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IFolderRepository folderRepository,
            ICollaboratorRepository collaboratorRepository,
            IFileRepository fileRepository,
            PermissionServices permissionServices
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _logger = logger.CreateLogger<ProjectController>();
            _folderRepository = folderRepository;
            _collaboratorRepository = collaboratorRepository;
            _fileRepository = fileRepository;
            _permissionServices = permissionServices;
        }

        [HttpGet("{id}", Name = "GetFile")]
        public IActionResult GetById(long id)
        {
            var file = _fileRepository.Find(id);
            return new ObjectResult(file);
        }

        [HttpPost]
        public IActionResult Create([FromBody] File item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var parent = _folderRepository.Find(item.ParentID);
            if (parent == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            if (_permissionServices.HasWriteAccess(parent, userId))
            {
                return new UnauthorizedResult();
            }

            _fileRepository.Add(item);
            return CreatedAtAction("GetFile", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] File item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            var file = _fileRepository.Find(id);

            if (file == null)
            {
                return NotFound();
            }
            else if (!_permissionServices.HasWriteAccess(file, userId))
            {
                return new UnauthorizedResult();
            }

            file.Content = item.Content;
            file.Name = item.Name;
            file.ParentID = item.ParentID;
            file.Syntax = item.Syntax;
            _fileRepository.Update(file);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var file = _fileRepository.Find(id);
            if (file == null)
            {
                return NotFound();
            }

            if (_permissionServices.HasWriteAccess(file, userId))
            {
                _fileRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
