using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Students.Logic
{
  public class PasswordHasher : IPasswordHasher
  {
    private const int SaltSize = 16;
    private const int SaltIterations = 10_000;
    private const int NumberBytesReq = 256 / 8;

    public string Hash(string password)
    {
      if (string.IsNullOrEmpty(password))
      {
        throw new VerificationException();
      }

      var salt = GetSalt();

      var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: SaltIterations,
        numBytesRequested: NumberBytesReq));

      return ConcatToHash(salt, hashed);
    }

    public bool Compare(string hash, string password)
    {
      var parts = hash.Split(".");

      if (parts.Length != 3)
      {
        throw new VerificationException();
      }

      var iterations = Convert.ToInt16(parts[0]);
      var salt = Convert.FromBase64String(parts[1]);
      var hashed = Convert.FromBase64String(parts[2]);

      var algorithm =
        KeyDerivation.Pbkdf2(
          password,
          salt,
          KeyDerivationPrf.HMACSHA1,
          iterations,
          NumberBytesReq);

      return algorithm.SequenceEqual(hashed);
    }

    private static byte[] GetSalt()
    {
      var salt = new byte[SaltSize];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(salt);
      return salt;
    }

    private static string ConcatToHash(byte[] salt, string hashed)
    {
      return $"{SaltIterations}.{Convert.ToBase64String(salt)}.{hashed}";
    }
  }
}
