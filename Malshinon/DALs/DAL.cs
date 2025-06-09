using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Entities;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Malshinon.DALs
{
    internal class DAL
    {
        private string connectionStr = "server=localhost;user=root;password=;database=MalshinonDB";
        private MySqlConnection _conn;

        public MySqlConnection Get_conn() => this._conn;

        public MySqlConnection OpenConnection()
        {
            if (_conn == null)
            {
                _conn = new MySqlConnection(connectionStr);
            }
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
                Console.WriteLine("connected succecfuly");
            }
            return _conn;
        }
        public void CloseConnection()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
                _conn = null;
            }
        }


        public List<Person> GetAllPeople()
        {
            List<Person> people = new List<Person>();

            return people;
        }
        public List<Report> GetAllReports()
        {
            List<Report> reports = new List<Report>();

            return reports; 
        }


        

        public void AddPersonToDB(string Fname, string Lname, string SecretCode, string status)
        {
            if (!_statusOK(status))
            {
                Console.WriteLine("this status is not alloud");
                return;
            }
            try
            {
                OpenConnection();
                string query = "INSERT INTO people (first_name, last_name, secret_code, type) " +
                    "VALUES (@Fname, @Lname, @Scode, @Type)";
                using (var cmd = new MySqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    cmd.Parameters.AddWithValue("@Lname", Lname);
                    cmd.Parameters.AddWithValue("@Scode", SecretCode);
                    cmd.Parameters.AddWithValue("@Type", status);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{Fname} {Lname} was added");
                    }
                    else
                    {
                        Console.WriteLine("something went wrong");
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
                CloseConnection();
            }
        }
        public void UptateStatus(string Fname, string status)
        {
            if (_statusOK(status))
            {
                try
                {
                    OpenConnection();
                    string query = "UPDATE people SET type = @Type WHERE first_name = @Fname";
                    using (var cmd = new MySqlCommand(query, _conn))
                    {
                        cmd.Parameters.AddWithValue("@Fname", Fname);
                        cmd.Parameters.AddWithValue("@Type", status);
                        cmd.ExecuteNonQuery();
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
                    CloseConnection();
                }
            }
        }
        public bool _statusOK(string status)
        {
            bool statusOK = false;
            string[] statuses = { "reporter", "target", "both", "potential_agent" };
            for (int i = 0; i < statuses.Length; i++)
            {
                if (status == statuses[i])
                {
                    statusOK = true;
                }
            }
            return statusOK;
        }
        public void IncreseNumReports(string Fname)
        {

        }
        public void IncreseNumMentions(string Fname)
        {

        }
        public int GetIdByFName(string Fname)
        {
            int Id = 0;
            try
            {
                OpenConnection();
                string query = "SELECT id FROM people WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = reader.GetInt32("id");
                        }
                        else
                        {
                            Console.WriteLine($"{Fname} coudent be found");
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
                CloseConnection();
            }
            return Id;
        }
        public void AddReportToDB(int reporterId, int targetId, string text)
        {
            try
            {
                OpenConnection();
                string query = "INSERT INTO IntelReports (reporter_id, target_id, text) " +
                    "VALUES (@reporter_id, @target_id, @text)";
                using (var cmd = new MySqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("@reporter_id", reporterId);
                    cmd.Parameters.AddWithValue("@target_id", targetId);
                    cmd.Parameters.AddWithValue("@text", text);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{reporterId} added on {targetId} areport");
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
                CloseConnection();
            }
        }

    }
}
