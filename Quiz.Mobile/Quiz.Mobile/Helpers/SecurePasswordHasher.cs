using Newtonsoft.Json;
using Quiz.Mobile.Shared.DTOs;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Quiz.Mobile.Helpers
{
    public static class SecurePasswordHasher
    {
        /// <summary>
        /// Długość ciągu zaburzająceg tzw. soli
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Długość rezultatu funkcji skrótu
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Utworzenie hash'u hasła.
        /// </summary>
        /// <param name="password">Hasło.</param>
        /// <param name="iterations">Liczba iteracji.</param>
        /// <returns>'Skrót' hasła.</returns>
        public static string Hash(string password, int iterations)
        {
            // Utworzenie soli
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Utworzenie sygnatury hasła tzw. hash'u
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Mieszanie soli i hash'u
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Zamiana do base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Umieszczenie dodatkowo informacji o liczbie iteracji
            return string.Format("{0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Utworzenie sygnatury hasła (hash'u) z wykorzystaniem 10000 iteracji
        /// Domyślne wywołanie bez podania liczby iteracji.
        /// </summary>
        /// <param name="password">Hasło.</param>
        /// <returns>Hash hasła.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Porównanie sygnatury (hash'u) z hasłem w formie ciągu znaków
        /// </summary>
        /// <param name="password">Hasło.</param>
        /// <param name="hashedPassword">Hash.</param>
        /// <returns>Prawda jeżeli równe. W przeciwnym wypadku fałsz.</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Wyodrębnienie zakodowanego hash'a oraz liczby iteracji
            var splittedHashString = hashedPassword.Split('$');
            if (!int.TryParse(splittedHashString[0], out int iterations))
                throw new NotSupportedException("Nie udało się wyodrębnić " +
                    "liczby iteracji z sygnatury hasła.");

            var base64Hash = splittedHashString[1];

            // Zamiana Base64 na tablicę bajtów
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Pobranie wielkości soli
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Utworzenie hash'u dla podanego ciągu znaków
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Porównanie obu sygnatur znak po znaku
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }

        public static string Encrypt(SimpleUserDto userDto)
        {
            var key = Encoding.UTF8.GetBytes(QuizApiSettings.QuizApiKey);
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                //generowanie wektora inicjalizującego
                aes.GenerateIV();
                //pobranie wektora inicjalizującego
                var iv = aes.IV;
                using (var encryptor = aes.CreateEncryptor())
                {
                    var plainBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userDto));
                    var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    var cipherText = Convert.ToBase64String(cipherBytes);
                    var ivText = Convert.ToBase64String(iv);

                    return $"{ivText}:{cipherText}";
                }
            }
        }
    }
}

