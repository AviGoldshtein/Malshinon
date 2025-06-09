using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Utils
{
    internal class SecretCodeGenerator
    {
        private string Code = "aaaaa";

        public string GenerateCode(int index = 4)
        {
            char charToChange = Code[index];
            if (charToChange == 'z')
            {
                return GenerateCode(index - 1);
            }
            else
            {
                int intNum = Convert.ToInt32(charToChange) + 1;
                char chrUpdated = (char)intNum;
                string newCode = "";
                for (int i = 0; i < index; i++)
                {
                    newCode += Code[i];
                }
                newCode += chrUpdated;
                for (int i = index; i < Code.Length; i++)
                {
                    newCode += Code[i];
                }
                Code = newCode;
                return newCode;
            }
        }
    }
}
