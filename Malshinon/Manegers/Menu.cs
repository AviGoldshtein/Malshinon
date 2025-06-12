using Malshinon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Manegers
{
    internal static class Menu
    {
        
        public static void ShowMenu(MainManeger maneger)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("1. Add report to DB\n" +
                    "2. Add new person to DB\n" +
                    "3. show all people\n" +
                    "4. show all reports\n" +
                    "5. show reports linked to a specific reporter\n" +
                    "6. show reports linked to a specific target\n" +
                    "7. show people of a specific type\n" +
                    "8. get secret code by full name\n" +
                    "9. show all alerts\n" +
                    "1000. to exit\n");
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        maneger.AddReport();
                        break;
                    case "2":
                        maneger.AddPersonMnualy();
                        break;
                    case "3":
                        Person.PrintListPouple(maneger.DalPerson.RetrieveAllPeople());
                        break;
                    case "4":
                        Report.PrintListReports(maneger.DalReport.showAllReports());
                        break;
                    case "5":
                        Report.PrintListReports(maneger.DalReport.showReportsForPerson("reporter_id"));
                        break;
                    case "6":
                        Report.PrintListReports(maneger.DalReport.showReportsForPerson("target_id"));
                        break;
                    case "7":
                        Person.PrintListPouple(maneger.DalPerson.RetrievePeopleOfType(typeOptions()));
                        break;
                    case "8":
                        Console.WriteLine(maneger.DalPerson.GetSecretCodeByName());
                        break;
                    case "9":
                        Alert.PrintListAlerts(maneger.DAalAlerts.RetrieveAllAlerts());
                        break;
                    case "1000":
                        Console.WriteLine("have a good day");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("invalid input");
                        break;
                }
                if (running)
                {
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        public static string typeOptions()
        {
            Console.WriteLine("1. reporters\n" +
                "2. targets\n" +
                "3. both\n" +
                "4. potential agets\n");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    return "reporter";
                case "2":
                    return "target";
                case "3":
                    return "both";
                case "4":
                    return "potential_agent";
            }
            return typeOptions();
        }
    }
}
