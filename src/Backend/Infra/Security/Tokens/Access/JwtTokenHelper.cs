using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Security.Tokens.Access;

public abstract class JwtTokenHelper
{
    protected static SymmetricSecurityKey ConvertToSecurityKey(string key)
    {
        var bytes = Encoding.UTF8.GetBytes(key);
        return new SymmetricSecurityKey(bytes);
    }
}