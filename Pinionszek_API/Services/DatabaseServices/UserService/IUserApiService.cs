using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.UserService
{
    public interface IUserApiService
    {
        public Task<IEnumerable<Friend>?> GetUserFriendsAsync(int idUser);
        public Task<UserSettings?> GetUserSettingsAsync(int idUser);
        public Task<User?> GetUserDataAsync(int idUser);
        public Task<IEnumerable<DetailedCategory>?> GetUserPaymentCategoriesAsync(int idUser);
        public Task<DetailedCategory?> GetUserPaymentCategoryAsync(int idUser, int idDetailedCategory);
    }
}
