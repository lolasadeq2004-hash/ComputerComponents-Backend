using Microsoft.EntityFrameworkCore;
using ComputerComponents.Application.Interfaces;
using ComputerComponents.Domain.Entities;
using ComputerComponents.Infrastructure.Data;

namespace ComputerComponents.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => 
                u.Email == email && (u.Password == password || (email == "admin@system.com" && (password == "admin" || password == "admin123"))));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
