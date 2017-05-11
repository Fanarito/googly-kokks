using System.Collections.Generic;
using System.IdentityModel;

namespace Kokks.Models
{
    public interface IUserRepository
    {
        ApplicationUser FindByEmail(string email);
    }
}