using FluentAssertions;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using Pinionszek_API.Tests.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests
{
    public class UserApiServiceTests
    {
        [Fact]
        public async Task BudgetUtils_GetUserCategoriesAsync_ReturnsCategoriesOrNotfound()
        {
            //Arrange
            var dbContext = new InMemContext().GetDatabaseContext();
            var userApiService = new UserApiService(await dbContext);
            int idUser_1 = 1;
            int idUser_2 = 2;
            int idUser_3 = 3;
            int idUser_4 = 4;

            //Act
            var userCategories1 = await userApiService.GetUserCategoriesAsync(idUser_1);
            var userCategories2 = await userApiService.GetUserCategoriesAsync(idUser_2);
            var userCategories3 = await userApiService.GetUserCategoriesAsync(idUser_3);
            var userCategories4 = await userApiService.GetUserCategoriesAsync(idUser_4);

            //Assert
            userCategories1.Should().NotBeNullOrEmpty();
            userCategories1?.Count().Should().Be(6);
            userCategories1?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories1?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories2.Should().NotBeNullOrEmpty();
            userCategories2?.Count().Should().Be(4);
            userCategories2?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories2?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories3.Should().NotBeNullOrEmpty();
            userCategories3?.Count().Should().Be(3);
            userCategories3?
                .Where(uc => string.IsNullOrEmpty(uc.Name) ||
                    string.IsNullOrEmpty(uc.GeneralCategory.Name))
                .Should().BeNullOrEmpty();
            userCategories3?
                .Where(uc => uc.IdDetailedCategory <= 0 ||
                    uc.GeneralCategory.IdGeneralCategory <= 0)
                .Should().BeNullOrEmpty();

            userCategories4.Should().BeNullOrEmpty();

        }
    }
}
