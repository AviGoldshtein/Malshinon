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
    }
}
