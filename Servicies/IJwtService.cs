namespace Services
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email, string firstName, string lastName, string role);
    }
}
