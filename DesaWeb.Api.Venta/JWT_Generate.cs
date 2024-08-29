using System;
using System.Security.Cryptography;

class JWT_Generate
{
    static void Main()
    {
        var key = new byte[32];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetBytes(key);
            string secret = Convert.ToBase64String(key);
            Console.WriteLine(secret);
        }
    }

}
