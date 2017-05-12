using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Kokks.Models;
using Kokks.Services;
using Kokks.Handlers;
using System.Threading.Tasks;

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
        private FileHandler _fileHandler;

        public FileController(
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IFolderRepository folderRepository,
            ICollaboratorRepository collaboratorRepository,
            IFileRepository fileRepository,
            PermissionServices permissionServices,
            FileHandler fileHandler
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _logger = logger.CreateLogger<ProjectController>();
            _folderRepository = folderRepository;
            _collaboratorRepository = collaboratorRepository;
            _fileRepository = fileRepository;
            _permissionServices = permissionServices;
            _fileHandler = fileHandler;
        }

        [HttpGet("{id}", Name = "GetFile")]
        public IActionResult GetById(long id)
        {
            var file = _fileRepository.Find(id);
            if (file == null)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(HttpContext.User);
            if (_projectRepository.UserHasAccess(file.Parent.ProjectID, userId))
            {
                return new ObjectResult(file);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] File item)
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
            if (!_permissionServices.HasWriteAccess(parent.ProjectID, userId))
            {
                return Unauthorized();
            }
            else
            {
                _fileRepository.Add(item);
                await _fileHandler.Add(item.Id, item.Name, item.Content, item.Syntax, item.ParentID, parent.ProjectID);
                return CreatedAtAction("GetFile", new { id = item.Id }, item);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] File item)
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
                return Unauthorized();
            }
            else
            {
                file.Content = item.Content;
                file.Name = item.Name;
                file.ParentID = item.ParentID;
                file.Syntax = item.Syntax;
                _fileRepository.Update(file);
                await _fileHandler.Add(file.Id, file.Name, file.Content, file.Syntax, file.ParentID, file.Parent.ProjectID);
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var file = _fileRepository.Find(id);
            if (file == null)
            {
                return NotFound();
            }

            if (_permissionServices.HasWriteAccess(file, userId))
            {
                await _fileHandler.Remove(file.Id, file.ParentID, file.Parent.ProjectID);
                _fileRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}
