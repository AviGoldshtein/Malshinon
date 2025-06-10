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

                        break;
                    case "1000":
                        Console.WriteLine("have a good day");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("invalid input");
                        break;
                }
            }
        }
    }
}
