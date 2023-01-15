
using GhostLibrary.Models;
using System.Security.Claims;

namespace ChatAppAPI.Helpers
{
    public interface IJwtService
    {
        CredentialsModel Credentials(ClaimsIdentity claims);
        string GenerateToken(CredentialsModel loginCredentials);
    }
}