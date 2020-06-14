using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyNote.Core
{
    public class BaseMySqlDbConnection : IDisposable
    {
        MySqlConnection _readMySqlConnection;

        public MySqlConnection ConnectionForSelectStatement { get { return _readMySqlConnection; } }

        public readonly string ConnectionString;


        //Constructor
        public BaseMySqlDbConnection(string connectionStringOrConnectionName)
        {
            System.Configuration.ConnectionStringSettings xmlConfiguration
                = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringOrConnectionName];

            if (xmlConfiguration == null)
            {
                BuildJsonConfig();

                string jsonConnString = _jsonConfiguration["ConnectionStrings:" + connectionStringOrConnectionName];

                if (string.IsNullOrEmpty(jsonConnString))
                {
                    ConnectionString = connectionStringOrConnectionName;
                }
                else
                {
                    ConnectionString = jsonConnString;
                }
            }
            else
            {
                ConnectionString = xmlConfiguration.ConnectionString;
            }
        }

        //open connection to database
        bool OpenReadConnection()
        {
            _readMySqlConnection = _readMySqlConnection ?? new MySqlConnection(ConnectionString);

            if (_readMySqlConnection.State == ConnectionState.Closed)
            {
                _readMySqlConnection.Open();
            }

            return _readMySqlConnection.State == ConnectionState.Open;
        }

        //Close connection
        bool CloseReadConnection()
        {
            if (_readMySqlConnection.State == ConnectionState.Open)
            {
                _readMySqlConnection.Close();
            }

            return _readMySqlConnection.State == ConnectionState.Closed;
        }

        public int ExecuteNonQuery(string query)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandTimeout = 120;
                //Execute command
                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ": " + query);
                    return -1;
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.CommandTimeout = 120;
                //Execute command
                try
                {
                    conn.Open();
                    return await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ": " + query);
                    return -1;
                }
            }
        }

        public object ExecuteScalar(string query)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Execute command
                try
                {
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ": " + query);
                    return -1;
                }
            }
        }

        public async Task<object> ExecuteScalarAsync(string query)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                //Execute command
                try
                {
                    conn.Open();
                    return await cmd.ExecuteScalarAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ": " + query);
                    return null;
                }
            }
        }

        public List<T> Select<T>(string query) where T : class
        {
            if (this.OpenReadConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _readMySqlConnection);
                try
                {
                    return cmd.ExecuteReader().MapToEntities<T>().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ": " + query);
                }
                finally
                {
                    //close connection
                    this.CloseReadConnection();
                }
            }

            return new List<T>();
        }

        //Select statement
        public DataTable Select(string query)
        {
            DataTable datatable = new DataTable();
            //Open connection
            if (this.OpenReadConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, _readMySqlConnection);
                try
                {
                    datatable.Load(cmd.ExecuteReader());
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    //close connection
                    this.CloseReadConnection();
                }
            }
            return datatable;
        }

        public void Dispose()
        {
            CloseReadConnection();
            _readMySqlConnection = null;
        }

        IConfigurationRoot _jsonConfiguration;

        void BuildJsonConfig()
        {
            if (_jsonConfiguration != null) return;

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), optional: false, reloadOnChange: true);

            this._jsonConfiguration = builder.Build();
        }
    }

}
