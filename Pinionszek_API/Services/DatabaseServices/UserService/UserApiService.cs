using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;

namespace Pinionszek_API.Services.DatabaseServices.UserService
{
    public class UserApiService : IUserApiService
    {
        private readonly ProdDbContext _dbContext;

        public UserApiService(ProdDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
