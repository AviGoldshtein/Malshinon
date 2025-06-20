﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.databases;
using Malshinon.Entities;
using Malshinon.Utils;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using static System.Net.Mime.MediaTypeNames;

namespace Malshinon.DALs
{
    internal class DALperson
    {
        DALreport DalReport = new DALreport();
        DALvalidator DALvalidator = new DALvalidator();
        DBconnectionMalshinon dbConnection = new DBconnectionMalshinon();

        public bool AddPersonToDB(Person person)
        {
            if (!_statusOK(person.Type))
            {
                Console.WriteLine("this status is not alloud");
                return false;
            }
            try
            {
                string firstName = Convert.ToString(char.ToUpper(person.Fname[0])) + person.Fname.Substring(1);
                string lastName = Convert.ToString(char.ToUpper(person.Lname[0])) + person.Lname.Substring(1);

                dbConnection.OpenConnection();
                string query = "INSERT INTO people (first_name, last_name, secret_code, type) " +
                    "VALUES (@Fname, @Lname, @Scode, @Type)";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", firstName);
                    cmd.Parameters.AddWithValue("@Lname", lastName);
                    cmd.Parameters.AddWithValue("@Scode", person.SecretCode);
                    cmd.Parameters.AddWithValue("@Type", person.Type);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{person.Fname} {person.Lname} was added");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("something went wrong while adding the person");
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
            return false;
        }
        public void UptateStatus(string Fname, string status)
        {
            if (_statusOK(status))
            {
                try
                {
                    dbConnection.OpenConnection();
                    string query = "UPDATE people SET type = @Type WHERE first_name = @Fname";
                    using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
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
                    dbConnection.CloseConnection();
                }
            }
        }
        private bool _statusOK(string status)
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
            int currentNumReports = _GetCurrentNumReportsByName(Fname);
            int numReports = currentNumReports + 1;
            _UpdateNumReports(Fname, numReports);

            double avrageLenReports = DalReport.GetAvrageLenReports(Fname);
            if (numReports >= 10 && avrageLenReports >= 100)
            {
                Console.WriteLine($"{Fname} is a potential agent");
                UptateStatus(Fname, "potential_agent");
            }

        }
        public int IncreseNumMentions(string Fname)
        {
            int currentNumMentions = _GetCurrentNumMentionsByName(Fname);
            int NumMentions = currentNumMentions + 1;
            _UpdateNumMentions(Fname, NumMentions);

            return NumMentions;
        }
        private int _GetCurrentNumMentionsByName(string Fname)
        {
            int currentNumMentions = -1;
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT num_mentions FROM people WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentNumMentions = reader.GetInt32("num_mentions");
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
                dbConnection.CloseConnection();
            }
            return currentNumMentions;
        }
        private int _GetCurrentNumReportsByName(string Fname)
        {
            int currentNumReports = -1;
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT num_reports FROM people WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", $"{Fname}");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentNumReports = reader.GetInt32("num_reports");
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
                dbConnection.CloseConnection();
            }
            return currentNumReports;
        }
        private void _UpdateNumMentions(string Fname, int NumMentions)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "UPDATE people SET num_mentions = @num_mentions WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    cmd.Parameters.AddWithValue("@num_mentions", NumMentions);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{Fname} updated to {NumMentions} Mentions");
                    }
                    else
                    {
                        Console.WriteLine("Update Num Mentions failed");
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
        private void _UpdateNumReports(string Fname, int numReports)
        {
            try
            {
                dbConnection.OpenConnection();
                string query = "UPDATE people SET num_reports = @num_reports WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@Fname", Fname);
                    cmd.Parameters.AddWithValue("@num_reports", numReports);
                    int effected = cmd.ExecuteNonQuery();
                    if (effected > 0)
                    {
                        Console.WriteLine($"{Fname} updated to {numReports} reports");
                    }
                    else
                    {
                        Console.WriteLine("Update Num Reports failed");
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
        public int GetIdByFName(string Fname)
        {
            int Id = 0;
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT id FROM people WHERE first_name = @Fname";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
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
                dbConnection.CloseConnection();
            }
            return Id;
        }
        public List<Person> RetrieveAllPeople()
        {
            List<Person> AllPoeple = new List<Person>();
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM people";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string first_name = reader.GetString("first_name");
                            string last_name = reader.GetString("last_name");
                            string secret_code = reader.GetString("secret_code");
                            string type = reader.GetString("type");
                            int num_reports = reader.GetInt32("num_reports");
                            int num_mentions = reader.GetInt32("num_mentions");

                            Person person = new Person
                            {
                                Id = id,
                                Fname = first_name,
                                Lname = last_name,
                                SecretCode = secret_code,
                                Type = type,
                                NumOfRports = num_reports,
                                NumOfMentions = num_mentions
                            };

                            AllPoeple.Add(person);
                            //Console.WriteLine($"id: {id}\n" +
                            //    $"first_name: {first_name}\n" +
                            //    $"last_name: {last_name}\n" +
                            //    $"secret_code: {secret_code}\n" +
                            //    $"type: {type}\n" +
                            //    $"num_reports: {num_reports}\n" +
                            //    $"num_mentions: {num_mentions}\n");
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
            return AllPoeple;
        }
        public List<Person> RetrievePeopleOfType(string typeasked)
        {
            List<Person> metchedPoeple = new List<Person>();
            try
            {
                dbConnection.OpenConnection();
                string query = "SELECT * FROM people WHERE type = @type";
                using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                {
                    cmd.Parameters.AddWithValue("@type", typeasked);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string first_name = reader.GetString("first_name");
                            string last_name = reader.GetString("last_name");
                            string secret_code = reader.GetString("secret_code");
                            string type = reader.GetString("type");
                            int num_reports = reader.GetInt32("num_reports");
                            int num_mentions = reader.GetInt32("num_mentions");

                            Person person = new Person
                            {
                                Id = id,
                                Fname = first_name,
                                Lname = last_name,
                                SecretCode = secret_code,
                                Type = type,
                                NumOfRports = num_reports,
                                NumOfMentions = num_mentions
                            };

                            metchedPoeple.Add(person);
                            //Console.WriteLine($"id: {id}, first_name: {first_name}, last_name: {last_name}, secret_code: {secret_code}, type: {type}, num_reports: {num_reports}, num_mentions: {num_mentions}");
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
            return metchedPoeple;
        }
        public string GetSecretCodeByName()
        {
            string secretCode = "";
            Console.WriteLine("enter the name:");
            string Fname = Console.ReadLine();
            
            if (!DALvalidator.EnsurePersonExeist(Fname))
            {
                Console.WriteLine($"{Fname} dosnt exeist");
            }
            else
            {
                try
                {
                    dbConnection.OpenConnection();
                    string query = "SELECT secret_code FROM people WHERE first_name = @Fname";
                    using (var cmd = new MySqlCommand(query, dbConnection.Get_conn()))
                    {
                        cmd.Parameters.AddWithValue("@Fname", Fname);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                secretCode = reader.GetString("secret_code");
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
            return secretCode;
        }
    }
}
