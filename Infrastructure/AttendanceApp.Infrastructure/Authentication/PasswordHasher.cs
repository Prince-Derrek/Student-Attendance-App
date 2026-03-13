using AttendanceApp.Application.Interfaces;
using BCrypt.Net;

namespace AttendanceApp.Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, hashType: HashType.SHA384);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, hashType: HashType.SHA384);
        }
    }
}