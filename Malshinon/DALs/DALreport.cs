using Malshinon.databases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DALs
{
    internal class DALreport
    {
        DBconnectionMalshinon dbConnection = new DBconnectionMalshinon();
        DALvalidator DalValidator = new DALvalidator();

        public void AddReportToDB(int reporterId, int targetId, string text)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "INSERT INTO IntelReports (reporter_id, target_id, text) " +
                    "VALUES (@reporter_id, @target_id, @text)";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    cmd.Parameters.AddWithValue("@text", text);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{reporterId} added a report on {targetId}");
                    }
                    else
                    {
                        Console.WriteLine("something went wrong in the report");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public double GetAvrageLenReports(string Fname)
        {
            int sum = 0;
            int counter = 0;
            double avrage = 0;
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT i.text " +
                    "FROM IntelReports AS i " +
                    "JOIN people AS p " +
                    "ON i.reporter_id = p.id " +
                    "WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string text = reader.GetString("text");
                            counter++;
                            sum += text.Length;

                            //Console.WriteLine(reader.GetString("text"));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            if (counter > 0) avrage = sum / counter;
            return avrage;
        }
        public void showAllReports()
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM IntelReports";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            int reporter_id = reader.GetInt32("reporter_id");
                            int target_id = reader.GetInt32("target_id");
                            string text = reader.GetString("text");
                            DateTime timestamp = reader.GetDateTime("timestamp");

                            Console.WriteLine($"id: {id}, reporter_id: {reporter_id}, target_id: {target_id}, text: {text}, timestamp: {timestamp}");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public void showReportsForPerson(string type)
        {
            Console.WriteLine("Enter the name that you want to look for");
            string name = Console.ReadLine();

            if (DalValidator.EnsurePersonExeist(name))
            {
                if (type == "reporter_id" || type == "target_id")
                {
                    _showReportsByName(name, type);
                }
            }
            else
            {
                Console.WriteLine($"{name} coudnt be found");
            }
        }
        private void _showReportsByName(string name, string type)
        {
            if (!(type == "reporter_id" || type == "target_id"))
            {
                Console.WriteLine("something was wrong");
                return;
            }
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT i.id, i.reporter_id, i.target_id, i.text, i.timestamp " +
                    "FROM IntelReports AS i " +
                    "JOIN people AS p " +
                    $"ON p.id = i.{type} " +
                    "WHERE p.first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            int reporter_id = reader.GetInt32("reporter_id");
                            int target_id = reader.GetInt32("target_id");
                            string text = reader.GetString("text");
                            DateTime timestamp = reader.GetDateTime("timestamp");

                            Console.WriteLine($"id: {id}, reporter_id: {reporter_id}, target_id: {target_id}, text: {text}, timestamp: {timestamp}");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
    }
}
