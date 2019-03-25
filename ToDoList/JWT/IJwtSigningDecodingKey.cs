using Microsoft.IdentityModel.Tokens;

namespace ToDoList.JWT
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
