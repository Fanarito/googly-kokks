using System;
using System.Collections.Generic;
using System.Linq;
using Kokks.Data;
using Microsoft.EntityFrameworkCore;

namespace Kokks.Models
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser FindByEmail(string email)
        {
            email = email.ToUpper();
            return _context.Users.FirstOrDefault(u => u.NormalizedEmail == email);
        }
    }
}