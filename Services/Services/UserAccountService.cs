using Microsoft.EntityFrameworkCore;
using Repositories.Interface;
using Repositories.Models;

namespace Services.Services
{
    public interface IAccountService
    {
        Task<UserAccount?> GetUserAccount(string email, string password);
    }

    public class UserAccountService : IAccountService
    {
        private readonly IGenericRepository<UserAccount> _accountRepository;
        public UserAccountService(IGenericRepository<UserAccount> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<UserAccount?> GetUserAccount(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userAccount = await _accountRepository
                .GetSet()
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            return userAccount;
        }
    }
}
