using System;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        // 64 bytes => 512 bits (secure for HMAC-SHA256)
        var keyBytes = RandomNumberGenerator.GetBytes(64);
        var base64Key = Convert.ToBase64String(keyBytes);

        Console.WriteLine();
        Console.WriteLine("✅ Generated secure JWT key (base64):");
        Console.WriteLine(base64Key);
        Console.WriteLine();
        Console.WriteLine("👉 Add to your .env (or user-secrets) as:");
        Console.WriteLine($"Jwt__Key={base64Key}");
    }
}
