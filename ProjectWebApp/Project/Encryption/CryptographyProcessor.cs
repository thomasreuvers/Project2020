﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.Encryption
{
    public class CryptographyProcessor
    {
        public string CreateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public string GenerateHash(string input, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(input + salt);

            using var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyHashedPassword(string plainTextInput, string hash, string salt)
        {
            var compareHash = GenerateHash(plainTextInput, salt);
            return compareHash.Equals(hash);
        }
        
    }
}