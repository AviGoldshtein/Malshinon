using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALs;
using Malshinon.Utils;
using MySqlX.XDevAPI;

namespace Malshinon.Manegers
{
    internal class MainManeger
    {
        DALvalidator DalValidator;
        public DALperson Dal;
        SecretCodeGenerator CodeGenerator;
        DALreport DalReport;

        public MainManeger(DALvalidator DV, DALperson dal, SecretCodeGenerator SCG, DALreport DR)
        {
            this.DalValidator = DV;
            this.Dal = dal;
            this.CodeGenerator = SCG;
            this.DalReport = DR;
        }
        public void AddReport()
        {
            Console.WriteLine("enter first name");
            string reporterFname = Console.ReadLine();
            Console.WriteLine("enter last name");
            string reporterLname = Console.ReadLine();

            bool isReporterStateValid = EnsureReportersState(reporterFname, reporterLname);
            if (!isReporterStateValid)
            {
                Console.WriteLine("the prosess of adding a report stoped");
                return;
            }


            Console.WriteLine("Enter your report, dont forget to mantion the targets name");
            string textReport = Console.ReadLine();

            var details = ExtractName(textReport);
            string targetFname = details.Fname;
            string targetLname = details.Lname;
            if (targetFname != "" && targetLname != "")
            {
                bool isTargetStateValid = EnsureTargetsState(targetFname, targetLname);
                if (!isTargetStateValid)
                {
                    Console.WriteLine("the prosess of adding a report stoped");
                    return;
                }

                InsertReport(reporterFname, targetFname, textReport);
            }
            else
            {
                Console.WriteLine("you must include in the report the name of the target");
            }
        }
        public bool EnsureReportersState(string reporterFname, string reporterLname)
        {
            if (!DalValidator.EnsurePersonExeist(reporterFname))
            {
                string code = CodeGenerator.GenerateCode();
                return Dal.AddPersonToDB(reporterFname, reporterLname, code, "reporter");
            }
            else
            {
                if (DalValidator.isTarget(reporterFname))
                {
                    Dal._UptateStatus(reporterFname, "both");
                    return true;
                }
            }
            return false;
        }
        public bool EnsureTargetsState(string targetFname, string targetLname)
        {
            if (!DalValidator.EnsurePersonExeist(targetFname))
            {
                string code = CodeGenerator.GenerateCode();
                return Dal.AddPersonToDB(targetFname, targetLname, code, "target");
            }
            else
            {
                if (DalValidator.isReporter(targetFname))
                {
                    Dal._UptateStatus(targetFname, "both");
                    return true;
                }
            }
            return false;
        }
        public (string Fname, string Lname) ExtractName(string text)
        {
            string[] words = text.Split(' ');
            string Fname = "";
            string Lname = "";
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (char.IsUpper(words[i][0]) && char.IsUpper(words[i + 1][0]))
                {
                    Fname = words[i];
                    Lname = words[i + 1];
                }
            }
            return (Fname, Lname);
        }
        public void InsertReport(string reporterFname, string targetFname, string textReport)
        {
            Dal.IncreseNumReports(reporterFname);
            Dal.IncreseNumMentions(targetFname);

            int reporterId = Dal.GetIdByFName(reporterFname);
            int targetId = Dal.GetIdByFName(targetFname);

            DalReport.AddReportToDB(reporterId, targetId, textReport);
        }
        public void AddPersonMnualy()
        {
            Console.WriteLine("Enter first name");
            string Fname = Console.ReadLine();
            Console.WriteLine("Enter last name");
            string Lname = Console.ReadLine();
            string code = CodeGenerator.GenerateCode();

            bool exist = DalValidator.EnsurePersonExeist(Fname);
            if (exist)
            {
                Console.WriteLine("this person already exist");
            }
            else
            {
                bool succsess = Dal.AddPersonToDB(Fname, Lname, code, "reporter");
                if (succsess)
                {
                    Console.WriteLine($"{Fname} {Lname} added successfully");
                }
                else
                {
                    Console.WriteLine($"there was a problem adding {Fname} {Lname}");
                }
            }
        }
        
    }
}
