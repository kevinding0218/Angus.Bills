using System;
using System.Threading.Tasks;
using Angus.Bills.Services.Users.Core.Entities;

namespace Angus.Bills.Services.Users.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task AddAsync(User customer);
        Task UpdateAsync(User customer);
    }
}