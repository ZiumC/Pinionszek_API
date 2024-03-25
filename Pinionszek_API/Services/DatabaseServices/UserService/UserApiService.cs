using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.UserService
{
    public class UserApiService : IUserApiService
    {
        private readonly ProdDbContext _dbContext;

        public UserApiService(ProdDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IEnumerable<Friend>?> GetUserFriends(int idUser)
        {
            throw new NotImplementedException();
        }
    }
}
