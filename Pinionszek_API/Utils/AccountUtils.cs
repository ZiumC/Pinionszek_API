using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace Pinionszek_API.Utils
{
    public class AccountUtils
    {
        private readonly IConfiguration _config;

        public AccountUtils(IConfiguration config)
        {
            _config = config;
        }

        public string MaskEmailString(string email)
        {
            string regexPattern = _config["Application:User:Account:HideEmailPattern"];
            string substitution = _config["Application:User:Account:Substitution"];
            RegexOptions options = RegexOptions.Multiline;
            Regex regex = new Regex(regexPattern, options);
            return regex.Replace(email, substitution);
        }

    }
}
