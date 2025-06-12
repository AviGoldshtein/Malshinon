using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALs;
using Malshinon.Entities;
using Malshinon.Utils;
using MySqlX.XDevAPI;

namespace Malshinon.Manegers
{
    internal class MainManeger
    {
        public DALalerts DAalAlerts;
        public DALvalidator DalValidator;
        public DALperson DalPerson;
        public DALreport DalReport;

        public MainManeger(DALvalidator DV, DALperson dal, DALreport DR, DALalerts DA)
        {
            this.DalValidator = DV;
            this.DalPerson = dal;
            this.DalReport = DR;
            this.DAalAlerts = DA;
        }
        public void AddReport()
        {
            Console.WriteLine("enter first name");
            string reporterFname = Console.ReadLine();
            Console.WriteLine("enter last name");
            string reporterLname = Console.ReadLine();

            bool isReporterValid = EnsureReportersState(reporterFname, reporterLname);
            if (!isReporterValid)
            {
                Console.WriteLine("the prosess of adding a report stoped");
                return;
            }

            Console.WriteLine("Enter your report, dont forget to mantion the targets name");
            string textReport = Console.ReadLine();

            var TargetDetails = ExtractName(textReport);
            string targetFname = TargetDetails.Fname;
            string targetLname = TargetDetails.Lname;

            if (targetFname != "" && targetLname != "")
            {
                bool IsTargetValid = EnsureTargetsState(targetFname, targetLname);
                if (!IsTargetValid)
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
                Person ReporterPerson = new Person
                {
                    Fname = reporterFname,
                    Lname = reporterLname,
                    SecretCode = SecretCodeGenerator.GenerateCode(),
                    Type = "reporter"
                };
                return DalPerson.AddPersonToDB(ReporterPerson);
            }
            else
            {
                if (DalValidator.isTarget(reporterFname))
                {
                    DalPerson.UptateStatus(reporterFname, "both");
                    return true;
                }
            }
            return true;
        }
        public bool EnsureTargetsState(string targetFname, string targetLname)
        {
            if (!DalValidator.EnsurePersonExeist(targetFname))
            {
                Person TargetPerson = new Person
                {
                    Fname = targetFname,
                    Lname = targetLname,
                    SecretCode = SecretCodeGenerator.GenerateCode(),
                    Type = "target"
                };
                return DalPerson.AddPersonToDB(TargetPerson);
            }
            else
            {
                if (DalValidator.isReporter(targetFname))
                {
                    DalPerson.UptateStatus(targetFname, "both");
                    return true;
                }
            }
            return true;
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
            DalPerson.IncreseNumReports(reporterFname);
            int numOfMentions =  DalPerson.IncreseNumMentions(targetFname);

            int reporterId = DalPerson.GetIdByFName(reporterFname);
            int targetId = DalPerson.GetIdByFName(targetFname);

            Report report = new Report
            {
                ReporterId = reporterId,
                TargetId = targetId,
                Text = textReport
            };

            DalReport.AddReportToDB(report);

            TriggerAlertIfNeeded(numOfMentions, targetFname, targetId);
        }
        public void TriggerAlertIfNeeded(int numOfMentions, string targetFname, int targetId)
        {
            if (numOfMentions % 5 == 0 && numOfMentions >= 20)
            {
                string textAlert = $"DANGER: {targetFname} has {numOfMentions} mentions";
                Console.WriteLine(textAlert);

                Alert alert = new Alert
                {
                    TargetId = targetId,
                    Reason = textAlert
                };

                DAalAlerts.InsertAlert(alert);
            }

            int NumReportsInTheLast15Minuts = DalReport.GetNumReportsInTheLast15Minuts(targetId);
            if (NumReportsInTheLast15Minuts >= 3)
            {
                string textAlert = $"DANGER: {targetFname} has {NumReportsInTheLast15Minuts} reports in the last 15 minuts";
                Console.WriteLine(textAlert);

                Alert alert = new Alert
                {
                    TargetId = targetId,
                    Reason = textAlert
                };

                DAalAlerts.InsertAlert(alert);
            }
        }
        public void AddPersonMnualy()
        {
            Console.WriteLine("Enter first name");
            string Fname = Console.ReadLine();
            Console.WriteLine("Enter last name");
            string Lname = Console.ReadLine();

            bool exist = DalValidator.EnsurePersonExeist(Fname);
            if (exist)
            {
                Console.WriteLine("this person already exist");
            }
            else
            {
                Person person = new Person
                {
                    Fname = Fname,
                    Lname = Lname,
                    SecretCode = SecretCodeGenerator.GenerateCode(),
                    Type = "reporter"
                };
                bool succsess = DalPerson.AddPersonToDB(person);
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
