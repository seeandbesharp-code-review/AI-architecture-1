using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zxcvbn;
using Enteties;

namespace Services
{
    public class passwordServices : IpasswordServices
    {
        public PassEntity GetStrength(string password)
        {
            if (password != null && password != "")
            {
                var result = Zxcvbn.Core.EvaluatePassword(password);
                if (result != null)
                {
                    int strengthPassword = result.Score;
                    PassEntity passEntity = new PassEntity();
                    passEntity.Password = password;
                    passEntity.Strength = strengthPassword;
                    return passEntity;
                }
            }
            return null;
        }

        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
