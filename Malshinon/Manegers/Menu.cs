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
                        maneger.Dal.showAllPeople();
                        break;
                    case "4":
                        //maneger.Dal.showAllReports();
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
    }
}
