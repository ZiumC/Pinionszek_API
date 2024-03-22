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

        public async Task<IEnumerable<DetailedCategory>?> GetUserCategoriesAsync(int idUser)
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
