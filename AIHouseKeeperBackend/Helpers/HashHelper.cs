using System.Security.Cryptography;
using System.Text;

namespace AIHouseKeeperBackend.Helpers;

public static class HashHelper
{
    public static string HashPassword(
        string password,
        string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        using var hmac = new HMACSHA256(keyBytes);
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashedBytes = hmac.ComputeHash(passwordBytes);
        var hashedPassword = Convert.ToBase64String(hashedBytes);
        return hashedPassword;
    }
}