using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALs;
using Malshinon.Utils;

namespace Malshinon.Manegers
{
    internal class MainManeger
    {
        DALvalidator DalValidator;
        DAL Dal;
        SecretCodeGenerator CodeGenerator;

        public MainManeger(DALvalidator DV, DAL dal, SecretCodeGenerator SCG)
        {
            this.DalValidator = DV;
            this.Dal = dal;
            this.CodeGenerator = SCG;
        }
        public void AddReport()
        {
            Console.WriteLine("enter first name");
            string reporterFname = Console.ReadLine();
            Console.WriteLine("enter last name");
            string reporterLname = Console.ReadLine();

            EnsureReportersState(reporterFname, reporterLname);

            Console.WriteLine("Enter your report, dont forget to mantion the targets name");
            string textReport = Console.ReadLine();

            var details = ExtractName(textReport);
            string targetFname = details.Fname;
            string targetLname = details.Lname;

            EnsureTargetsState(targetFname, targetLname);

            InsertReport(reporterFname, targetFname, textReport);




        }
        public void EnsureReportersState(string reporterFname, string reporterLname)
        {
            if (!DalValidator.EnsurePersonExeist(reporterFname))
            {
                string code = CodeGenerator.GenerateCode();
                Dal.AddPersonToDB(reporterFname, reporterLname, code, "reporter");
            }
            else
            {
                if (DalValidator.isTarget(reporterFname))
                {
                    Dal._UptateStatus(reporterFname, "both");
                }
            }
        }
        public void EnsureTargetsState(string targetFname, string targetLname)
        {
            if (!DalValidator.EnsurePersonExeist(targetFname))
            {
                string code = CodeGenerator.GenerateCode();
                Dal.AddPersonToDB(targetFname, targetLname, code, "target");
            }
            else
            {
                if (DalValidator.isReporter(targetFname))
                {
                    Dal._UptateStatus(targetFname, "both");
                }
            }
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

            Dal.AddReportToDB(reporterId, targetId, textReport);
        }
    }
}
