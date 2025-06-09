using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Person(string fname, string lname, string secretCode, string type, int id = 0, int numOfRports = 0, int numOfMentions = 0)
        {
            Id = id;
            Fname = fname;
            Lname = lname;
            SecretCode = secretCode;
            Type = type;
            NumOfRports = numOfRports;
            NumOfMentions = numOfMentions;
        }

        public int GetId() => this.Id;
        public string GetFname() => this.Fname;
        public string GetLname() => this.Lname;
        public string GetSecretCode() => this.SecretCode;
        public string GetType() => this.Type;
        public int GetNumOfRports() => this.NumOfRports;
        public int GetNumOfMentions() => this.NumOfMentions;
    }
}
