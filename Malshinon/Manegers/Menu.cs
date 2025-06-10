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
                Console.WriteLine("1. Add report to data base\n" +
                    "2. get the secret code by name\n" +
                    "1000. to exit\n");
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        maneger.AddReport();
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
