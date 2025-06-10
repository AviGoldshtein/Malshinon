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
            //DAL dAL = new DAL();
            //DALvalidator dALvalidator = new DALvalidator();
            //SecretCodeGenerator CodeGenerator = new SecretCodeGenerator();
            //dAL.OpenConnection();
            //string code = CodeGenerator.GenerateCode("aaaab");
            //Console.WriteLine(dALvalidator.EnsurePersonExeist("muchamady"));

            //Person person = new Person("muchamady", "alcerem", code, "reporter");
            //dAL.AddPersonToDB(person);
            //Menu menu = new Menu();
            DALvalidator dalValdator = new DALvalidator();
            DALperson dal = new DALperson();
            SecretCodeGenerator SCG = new SecretCodeGenerator();
            DALreport DR = new DALreport();
            MainManeger maneger = new MainManeger(dalValdator, dal, SCG, DR);
            Menu.ShowMenu(maneger);
            //Console.WriteLine(dal.GetAvrageLenReports("Avi"));





        }
    }
}
