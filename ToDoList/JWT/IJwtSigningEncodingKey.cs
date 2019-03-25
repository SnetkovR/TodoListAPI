using Microsoft.IdentityModel.Tokens;

namespace ToDoList.JWT
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}