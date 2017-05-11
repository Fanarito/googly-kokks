using Kokks.Models;

namespace Kokks.Services
{
    public class PermissionServices {
        private readonly ICollaboratorRepository _collaboratorRepository;

        public PermissionServices(
                              ICollaboratorRepository collaboratorRepository
                              )
        {
            _collaboratorRepository = collaboratorRepository;
        }

        public bool HasWriteAccess(Collaborator collaborator)
        {
            return (collaborator != null && (collaborator.Permission == Permissions.Owner ||
                                             collaborator.Permission == Permissions.ReadWrite));
        }

        public bool HasWriteAccess(long projectId, string userId)
        {
            var collaborator = _collaboratorRepository.Find(projectId, userId);
            return HasWriteAccess(collaborator);
        }

        public bool HasWriteAccess(Project p, string userId)
        {
            var collaborator = _collaboratorRepository.Find(p.Id, userId);
            return HasWriteAccess(collaborator);
        }

        public bool HasWriteAccess(File f, string userId)
        {
            var collaborator = _collaboratorRepository.Find(f.Parent.ProjectID, userId);
            return HasWriteAccess(collaborator);
        }

        public bool HasWriteAccess(Folder f, string userId)
        {
            var collaborator = _collaboratorRepository.Find(f.ProjectID, userId);
            return HasWriteAccess(collaborator);
        }
    }
}
