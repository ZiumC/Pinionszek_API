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

        public async Task<IEnumerable<Friend>?> GetUserFriends(int idUser)
        {
            return await _dbContext.Friends
                .Where(f => f.IdUser == idUser)
                .Join(_dbContext.Users,
                f => f.IdUser,
                u => u.IdUser,
                (f, u) => new Friend
                {
                    IdFriend = f.IdFriend,
                    FriendTag = f.FriendTag,
                    DateAdded = f.DateAdded,
                    User = new User
                    {
                        IdUser = u.IdUser,
                        Login = u.Login
                    }
                }).ToListAsync();
        }
    }
}
