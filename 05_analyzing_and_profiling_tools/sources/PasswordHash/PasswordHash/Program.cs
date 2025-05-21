using System.Security.Cryptography;

static class Program
{
    const int SaltLength = 16;
    const int HashLength = 20;
    const int Iterate = 20;

    static void Main(string[] args)
    {
        var salt = GenerateRandomBytes(20);
        Console.Write("Old: ");
        Console.WriteLine(GeneratePasswordHashUsingSalt("Hello, World!", salt));

        Console.Write("New: ");
        Console.WriteLine(GeneratePasswordHashUsingSaltNew("Hello, World!", salt));
        Console.ReadLine();
    }

    private static string GeneratePasswordHashUsingSaltNew(string passwordText, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, Iterate, HashAlgorithmName.SHA256);

        var hash = pbkdf2.GetBytes(20);

        var hashBytes = salt.Take(SaltLength).Concat(hash.Take(HashLength)).ToArray();

        return Convert.ToBase64String(hashBytes);
    }

    private static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
    {
        var iterate = 10000;

        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);

        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];

        Array.Copy(salt, 0, hashBytes, 0, 16);

        Array.Copy(hash, 0, hashBytes, 16, 20);

        var passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;
    }

    static byte[] GenerateRandomBytes(int size)
    {
        byte[] bytes = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(bytes);
        }
        return bytes;
    }
}