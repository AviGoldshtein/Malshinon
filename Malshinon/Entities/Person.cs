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
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string SecretCode { get; set; }
        public string Type { get; set; }
        public int NumOfRports { get; set; }
        public int NumOfMentions { get; set; }

        //public Person(string fname, string lname, string type = "reporter")
        //{
        //    Fname = fname;
        //    Lname = lname;
        //    SecretCode = SecretCodeGenerator.GenerateCode();
        //    Type = type;
        //    NumOfRports = 0;
        //    NumOfMentions = 0;
        //}

        public int GetId() => this.Id;
        public string GetFname() => this.Fname;
        public string GetLname() => this.Lname;
        public string GetSecretCode() => this.SecretCode;
        public string GetTypeName() => this.Type;
        public int GetNumOfRports() => this.NumOfRports;
        public int GetNumOfMentions() => this.NumOfMentions;

        public override string ToString()
        {
            return $"Id: {Id}\n" +
                $"Fname: {Fname}\n" +
                $"Lname: {Lname}\n" +
                $"SecretCode: {SecretCode}\n" +
                $"Type: {Type}\n" +
                $"NumOfRports: {NumOfRports}\n" +
                $"NumOfMentions: {NumOfMentions}\n";
        }

        public static void PrintListPouple(List<Person> people)
        {
            foreach(Person person in people)
            {
                Console.WriteLine(person);
            }
        }
    }
}
