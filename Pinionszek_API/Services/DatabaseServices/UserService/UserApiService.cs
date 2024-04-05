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

        public async Task<IEnumerable<Friend>?> GetUserFriendsAsync(int idUser)
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

        public async Task<UserSettings?> GetUserSettingsAsync(int idUser)
        {
            return await _dbContext.UserSettings
                .Where(us => us.IdUser == idUser)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserDataAsync(int idUser)
        {
            return await _dbContext.Users
                .Where(u => u.IdUser == idUser)
                .Select(u => new User
                {
                    IdUser = u.IdUser,
                    UserTag = u.UserTag,
                    Email = u.Email,
                    Login = u.Login,
                    Password = u.Password,
                    PasswordSalt = u.PasswordSalt,
                    RegisteredAt = u.RegisteredAt,
                    RefreshToken = u.RefreshToken,
                    LoginAttempts = u.LoginAttempts,
                    BlockedTo = u.BlockedTo
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DetailedCategory>?> GetUserPaymentCategoriesAsync(int idUser)
        {
            return await _dbContext.DetailedCategories
                .Where(dc => dc.IdUser == idUser)
                .Join(_dbContext.GeneralCategories,
                dc => dc.IdGeneralCategory,
                gc => gc.IdGeneralCategory,
                (dc, gc) => new DetailedCategory
                {
                    IdDetailedCategory = dc.IdDetailedCategory,
                    Name = dc.Name,
                    GeneralCategory = new GeneralCategory
                    {
                        IdGeneralCategory = gc.IdGeneralCategory,
                        Name = gc.Name,
                        IsDefault = gc.IsDefault
                    }
                }).ToListAsync();
        }
    }
}
