using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.DALs;
using Malshinon.Entities;
using Malshinon.Manegers;
using Malshinon.Utils;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DALvalidator dalValdator = new DALvalidator();
            DALperson dal = new DALperson();
            DALreport DR = new DALreport();
            DALalerts DA = new DALalerts();
            MainManeger maneger = new MainManeger(dalValdator, dal, DR, DA);
            Menu.ShowMenu(maneger);

        }
    }
}
