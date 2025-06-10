using Malshinon.databases;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.DALs
{
    internal class DALvalidator
    {
        DBconnectionMalshinon dbConnection = new DBconnectionMalshinon();

        public bool EnsurePersonExeist(string Fname)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM people WHERE first_name LIKE @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"%{Fname}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public bool isTarget(string Fname)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM people WHERE first_name LIKE @Fname AND type = 'target'";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"%{Fname}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }
        public bool isReporter(string Fname)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM people WHERE first_name = @Fname AND type = 'reporter'";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Sql Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }


    }
}
