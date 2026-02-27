using System.Security.Cryptography;

namespace Puush.Infrastructure.Utilities;

public static class RandomUtils
{
    private const string Alphabet =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GenerateSecureRandomString(int length = 16)
    {
        Span<byte> bytes = stackalloc byte[length];
        RandomNumberGenerator.Fill(bytes);

        Span<char> chars = stackalloc char[length];

        for (var i = 0; i < length; i++)
        {
            chars[i] = Alphabet[bytes[i] & (Alphabet.Length - 1)];
        }

        return new string(chars);
    }
}