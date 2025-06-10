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
                    "5. show reports by reporter name\n" +
                    "6. show reports by targets name\n" +
                    "7. show people of a specific type\n" +
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
                        maneger.DalPerson.showAllPeople();
                        break;
                    case "4":
                        maneger.DalReport.showAllReports();
                        break;
                    case "7":
                        maneger.DalPerson.ShowPoupleOfType(typeOptions());
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
