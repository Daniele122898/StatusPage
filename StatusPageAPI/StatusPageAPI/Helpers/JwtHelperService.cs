using System.Text;
using Microsoft.IdentityModel.Tokens;
using StatusPageAPI.Models.Configurations;

namespace StatusPageAPI.Helpers
{
    public class JwtHelperService
    {
        public SymmetricSecurityKey JwtKey { get; private set; }

        public JwtHelperService(AuthenticationConfig authSettings)
        {
            JwtKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Token));
        }
    }
}