﻿using Pinionszek_API.Models.DatabaseModel;

namespace Pinionszek_API.Services.DatabaseServices.UserService
{
    public interface IUserApiService
    {
        public Task<IEnumerable<Friend>?> GetUserFriends(int idUser);
    }
}