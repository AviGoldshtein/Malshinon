using Malshinon.databases;
using Malshinon.Entities;
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

        public void AddReportToDB(Report report)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "INSERT INTO IntelReports (reporter_id, target_id, text) " +
                    "VALUES (@reporter_id, @target_id, @text)";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", report.ReporterId);
                    cmd.Parameters.AddWithValue("@target_id", report.TargetId);
                    cmd.Parameters.AddWithValue("@text", report.Text);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{report.ReporterId} added a report on {report.TargetId}");
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
        public List<Report> showAllReports()
        {
            List<Report> reports = new List<Report>();
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

                            Report report = new Report
                            {
                                Id = id,
                                ReporterId = reporter_id,
                                TargetId = target_id,
                                Timestamp = timestamp,
                                Text = text,
                            };

                            reports.Add(report);

                            //Console.WriteLine($"id: {id}\n" +
                            //    $"reporter_id: {reporter_id}\n" +
                            //    $"target_id: {target_id}\n" +
                            //    $"timestamp: {timestamp}\n" +
                            //    $"text: {text}\n");
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
            return reports;
        }
        public List<Report> showReportsForPerson(string type)
        {
            List<Report> reports = new List<Report>();
            Console.WriteLine("Enter the name that you want to look for");
            string name = Console.ReadLine();

            if (DalValidator.EnsurePersonExeist(name))
            {
                if (type == "reporter_id" || type == "target_id")
                {
                    reports = _showReportsByName(name, type);
                }
            }
            else
            {
                Console.WriteLine($"{name} coudnt be found");
            }
            return reports;
        }  
        private List<Report> _showReportsByName(string name, string type)
        {
            List<Report> reports = new List<Report>();
            if (!(type == "reporter_id" || type == "target_id"))
            {
                Console.WriteLine("something was wrong");
                return reports;
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

                            Report report = new Report
                            {
                                Id = id,
                                ReporterId = reporter_id,
                                TargetId = target_id,
                                Text = text,
                                Timestamp = timestamp
                            };

                            reports.Add(report);

                            //Console.WriteLine($"id: {id}, reporter_id: {reporter_id}, target_id: {target_id}, text: {text}, timestamp: {timestamp}");
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
            return reports;
        }
        public int GetNumReportsInTheLast15Minuts(int personId)
        {
            int counter = 0;
            try
            {
                dbConnection.OpenConnection();
                string Query = "SELECT COUNT(*) AS count " +
                "FROM IntelReports " +
                "WHERE timestamp >= NOW() - INTERVAL 15 MINUTE " +
                "AND timestamp <= NOW() " +
                "AND target_id = @id";

                using (var cmd = new MySqlCommand(Query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@id", personId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            counter = reader.GetInt32("count");
                            Console.WriteLine(counter);
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
            return counter;
        }
    }
}
