using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Hash
{
    public static class HashUserPassword
    {
        private static readonly string SALTMAIN = "MyLittleShop";

        public static string DoHash(string UserId, string UserPassword, string UserToken)
        {
            //basic salting
            var salt1 = UserPassword + UserId + UserToken;
            var salt2 = salt1 + SALTMAIN;

            // Create a new instance of the hash crypto service provider.
            HashAlgorithm algorithm = new SHA256CryptoServiceProvider();

            // Convert the data to hash to an array of Bytes.
            byte[] byteValue = Encoding.UTF8.GetBytes(salt2);

            // Compute the Hash. This returns an array of Bytes.
            byte[] byteHash = algorithm.ComputeHash(byteValue);

            // Convert byte hash to string to use
            var hashedPassword = Convert.ToBase64String(byteHash);

            return hashedPassword;
        }
    }
}
