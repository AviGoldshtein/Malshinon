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
    internal class DALalerts
    {
        DBconnectionMalshinon dbConnection = new DBconnectionMalshinon();

        public void InsertAlert(Alert alert)
        {
            try
            {
                string Query = "INSERT INTO alerts (target_id, reason) VALUES (@target_id, @reason)";
                dbConnection.OpenConnection();
                using (var cmd = new MySqlCommand(Query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@target_id", alert.TargetId);
                    cmd.Parameters.AddWithValue("@reason", alert.Reason);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine("The Alert was saved");
                    }
                    else
                    {
                        Console.WriteLine("There was a problem saving the alert");
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"sql error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"general exception: {ex.Message}");
            }
        }
        public List<Alert> RetrieveAllAlerts()
        {
            List<Alert> alerts = new List<Alert>();
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM alerts";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            int target_id = reader.GetInt32("target_id");
                            DateTime created_at = reader.GetDateTime("created_at");
                            string reason = reader.GetString("reason");

                            Alert alert = new Alert
                            {
                                Id = id,
                                TargetId = target_id,
                                CreatedAt = created_at,
                                Reason = reason
                            };

                            alerts.Add(alert);
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
            return alerts;

        }
    }
}
