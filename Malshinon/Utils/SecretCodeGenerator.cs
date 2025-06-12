using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Utils
{
    internal static class SecretCodeGenerator
    {
        private static Faker faker = new Faker();
        public static string GenerateCode()
        {
            string secretCode = faker.Random.AlphaNumeric(10);
            return secretCode;
        }
    }
}
