using System;
using System.Security.Cryptography;

public static class EncryptionUtilities
{
    private const int SALT_SIZE = 8;
    private const int NUM_ITERATIONS = 1000;

    private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

    /// Creates a signature for a password.
    public static string CreatePasswordSalt(string password)
    {
        byte[] buf = new byte[SALT_SIZE];
        rng.GetBytes(buf);
        string salt = Convert.ToBase64String(buf);

        Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(password.Trim(), buf, NUM_ITERATIONS);
        string hash = Convert.ToBase64String(deriver2898.GetBytes(16));
        return salt + ':' + hash;
    }

    /// Validate if a password will generate the passed in salt:hash.
    public static bool IsPasswordValid(string password, string saltHash)
    {
        string[] parts = saltHash.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)

            return false;
        byte[] buf = Convert.FromBase64String(parts[0]);
        Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(password.Trim(), buf, NUM_ITERATIONS);
        string computedHash = Convert.ToBase64String(deriver2898.GetBytes(16));
        return parts[1].Equals(computedHash);
    }
}