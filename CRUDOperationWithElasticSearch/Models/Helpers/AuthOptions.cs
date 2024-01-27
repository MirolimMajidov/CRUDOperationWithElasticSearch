using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyUser.Models.Helpers;

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "your-secret-key-with-at-least-256-bits";   // ключ для шифрации
    public const int LIFETIME = 15; // время жизни токена - 15 минута
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
