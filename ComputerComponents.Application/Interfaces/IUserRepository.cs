using ComputerComponents.Domain.Entities;

namespace ComputerComponents.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User?> GetByEmailAsync(string email);
        Task<int> GetCountAsync();
    }
}
