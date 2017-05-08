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
    public class FileController : Controller 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger _logger;
        private readonly IFolderRepository _folderRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly IFileRepository _fileRepository;

        public FileController(
            IProjectRepository projectRepository,
            UserManager<ApplicationUser> userManager,
            ILoggerFactory logger,
            IFolderRepository folderRepository,
            ICollaboratorRepository collaboratorRepository,
            IFileRepository fileRepository
        )
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _logger = logger.CreateLogger<ProjectController>();
            _folderRepository = folderRepository;
            _collaboratorRepository = collaboratorRepository;
            _fileRepository = fileRepository;
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

            var userId = _userManager.GetUserId(HttpContext.User);
            var currentCollaborator = _collaboratorRepository.Find(item.Parent.ProjectID, userId);

            if (currentCollaborator == null || (currentCollaborator.Permission != Permissions.Owner
               && currentCollaborator.Permission != Permissions.ReadWrite))
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

            var currentCollaborator = _collaboratorRepository.Find(file.Parent.ProjectID, userId);

            if (currentCollaborator == null || (currentCollaborator.Permission != Permissions.Owner
               && currentCollaborator.Permission != Permissions.ReadWrite))
            {
                return new UnauthorizedResult();
            }

            if (!_projectRepository.UserHasAccess(file.Parent.ProjectID, userId))
            {
                return Unauthorized();
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


            var currentCollaborator = _collaboratorRepository.Find(file.Parent.ProjectID, userId);

            // You can only delete if you are a collaborator 
            // and own the project or have readWrite permission
            if (currentCollaborator != null || (currentCollaborator.Permission == Permissions.Owner
                || currentCollaborator.Permission == Permissions.ReadWrite))
            {
                _fileRepository.Remove(id);
                return NoContent();
            }

            return Unauthorized();
        }
    }
}