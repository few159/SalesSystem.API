﻿using System.Security.Cryptography;
using SalesSystem.Application.Interfaces.Services;

namespace SalesSystem.Infrastructure.Services;

public class Hasher : IHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32; // 256 bits
    private const int Iterations = 100_000;

    public string HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA256
        );

        var salt = algorithm.Salt;
        var key = algorithm.GetBytes(KeySize);

        // Formato: {iterations}.{salt}.{key}
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.', 3);
        if (parts.Length != 3)
            return false;

        var iterations = int.Parse(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256
        );

        var keyToCheck = algorithm.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
    }
}