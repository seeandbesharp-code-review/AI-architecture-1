using Services;
using Xunit;

namespace TestProject
{
    public class PasswordServicesTests
    {
        private readonly IpasswordServices _passwordServices;

        public PasswordServicesTests()
        {
            _passwordServices = new passwordServices();
        }

        [Fact]
        public void HashPassword_ReturnsDifferentStringThanInput()
        {
            // The stored hash must never equal the plain-text password
            var hash = _passwordServices.HashPassword("myPassword123!");
            Assert.NotEqual("myPassword123!", hash);
        }

        [Fact]
        public void HashPassword_SamePasswordProducesDifferentHashes()
        {
            // Each call generates a new random salt, so hashes differ
            var hash1 = _passwordServices.HashPassword("myPassword123!");
            var hash2 = _passwordServices.HashPassword("myPassword123!");
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ReturnsTrue_WhenPasswordMatchesHash()
        {
            var password = "myPassword123!";
            var hash = _passwordServices.HashPassword(password);
            Assert.True(_passwordServices.VerifyPassword(password, hash));
        }

        [Fact]
        public void VerifyPassword_ReturnsFalse_WhenPasswordDoesNotMatchHash()
        {
            var hash = _passwordServices.HashPassword("myPassword123!");
            Assert.False(_passwordServices.VerifyPassword("wrongPassword", hash));
        }
    }
}
