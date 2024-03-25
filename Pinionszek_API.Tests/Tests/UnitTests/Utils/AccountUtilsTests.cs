using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Pinionszek_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinionszek_API.Tests.Tests.UnitTests.Utils
{
    public class AccountUtilsTests
    {
        private readonly IConfiguration _config;
        public AccountUtilsTests()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    path: "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .Build();
        }


        [Fact]
        public async Task AccountUtils_MaskEmailString_ReturnsMaskedEmailOrThrowsExteption()
        {
            //Arrange
            var accountUtils = new AccountUtils(_config);
            string email_1 = "test@test.pl";
            string email_2 = "t@test.com";
            string email_3 = "te@ta.it";
            string email_4 = "test2@gmail.pl";
            string email_5 = "test231@o2.pl";

            //Act
            var maskedEmail_1 = accountUtils.MaskEmailString(email_1);
            var maskedEmail_2 = accountUtils.MaskEmailString(email_2);
            var maskedEmail_3 = accountUtils.MaskEmailString(email_3);
            var maskedEmail_4 = accountUtils.MaskEmailString(email_4);
            var maskedEmail_5 = accountUtils.MaskEmailString(email_5);

            //Assert
            maskedEmail_1.Should().Contain("*");
            maskedEmail_1.Split("@")[0].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(2);
            maskedEmail_1.Split("@")[1].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(2);

            maskedEmail_2.Should().Contain("*");
            maskedEmail_2.Split("@")[0].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(0);
            maskedEmail_2.Split("@")[1].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(2);

            maskedEmail_3.Should().Contain("*");
            maskedEmail_3.Split("@")[0].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(1);
            maskedEmail_3.Split("@")[1].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(1);

            maskedEmail_4.Should().Contain("*");
            maskedEmail_4.Split("@")[0].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(3);
            maskedEmail_4.Split("@")[1].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(3);

            maskedEmail_5.Should().Contain("*");
            maskedEmail_5.Split("@")[0].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(5);
            maskedEmail_5.Split("@")[1].ToCharArray()
                .Where(x => x == '*').Count().Should().Be(1);
        }
    }
}
