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
        DAL dal = new DAL();

        public bool EnsurePersonExeist(string Fname)
        {
            try
            {
                dal.OpenConnection();
                string query = "SELECT * FROM people WHERE first_name LIKE @Fname";
                using (var cmd = new MySqlCommand(query, dal.Get_conn()))
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
                dal.CloseConnection();
            }
        }
        public bool isTarget(string Fname)
        {
            try
            {
                dal.OpenConnection();
                string query = "SELECT * FROM people WHERE first_name LIKE @Fname AND type = 'target'";
                using (var cmd = new MySqlCommand(query, dal.Get_conn()))
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
                dal.CloseConnection();
            }
        }


    }
}
