using Enteties;

namespace Services
{
    public interface IpasswordServices
    {
        PassEntity GetStrength(string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}