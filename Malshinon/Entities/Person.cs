using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Utils;

namespace Malshinon.Entities
{
    internal class Person
    {
        int Id;
        string Fname;
        string Lname;
        string SecretCode;
        string Type;
        int NumOfRports;
        int NumOfMentions;

        public Person(string fname, string lname, string type = "reporter")
        {
            Fname = fname;
            Lname = lname;
            SecretCode = SecretCodeGenerator.GenerateCode();
            Type = type;
            NumOfRports = 0;
            NumOfMentions = 0;
        }

        public int GetId() => this.Id;
        public string GetFname() => this.Fname;
        public string GetLname() => this.Lname;
        public string GetSecretCode() => this.SecretCode;
        public string GetTypeName() => this.Type;
        public int GetNumOfRports() => this.NumOfRports;
        public int GetNumOfMentions() => this.NumOfMentions;
    }
}
