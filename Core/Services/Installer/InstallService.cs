using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using SharedKernel.Data;
using SharedKernel.Engines;
using SharedKernel.IServices;
using SharedKernel.Models;
using SharedKernel.Plugins;

namespace Core.Services.Installer
{
    public class InstallService : IInstallService
    {
        private readonly IEngine _engine;

        public InstallService(IEngine engine)
        {
            _engine = engine;
        }

        public void Run()
        {
            var model = new DbDataModel
            {
                SqlAuthenticationType = "windowsauthentication",
                SqlDatabaseName = "Emo23",
                SqlServerName = @".\SQLEXPRESS",
                SqlServerUsername = "",
                SqlServerPassword = ""
            };

            //try to create connection string
            var connectionString = CreateConnectionString(model.SqlAuthenticationType == "windowsauthentication",
                model.SqlServerName, model.SqlDatabaseName,
                model.SqlServerUsername, model.SqlServerPassword);

            var conf = new Config
            {
                ConnectionString = connectionString,
                SystemInstalled = true,
                Plugins = new List<PluginDescriptor>()
            };

            _engine.LoadnSaveConfigs(conf);
            CreateDatabase();
            InitializeDatabase();
        }
        //create database
        public virtual void CreateDatabase(string collation = "")
        {
            if (!SqlServerDatabaseExists(_engine.ConnectionString))
            {
                var errorCreatingDatabase = CreateDatabase(_engine.ConnectionString, collation);
                if (!string.IsNullOrEmpty(errorCreatingDatabase))
                    throw new Exception(errorCreatingDatabase);
            }
        }

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            try
            {
                //create tables
                var tables = _engine.Context.GenerateCreateScript();
                _engine.Context.ExecuteSqlScript(tables);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
  
        }

        protected virtual string CreateConnectionString(bool trustedConnection,
            string serverName, string databaseName,
            string username, string password, int timeout = 0, bool UseMars = false)
        {
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = trustedConnection,
                DataSource = serverName,
                InitialCatalog = databaseName
            };

            if (!trustedConnection)
            {
                builder.UserID = username;
                builder.Password = password;
            }

            builder.PersistSecurityInfo = false;
            if (UseMars)
            {
                builder.MultipleActiveResultSets = true;
            }

            if (timeout > 0)
            {
                builder.ConnectTimeout = timeout;
            }

            return builder.ConnectionString;
        }


        /// <summary>
        /// Creates a database on the server.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="collation">Server collation; the default one will be used if not specified</param>
        /// <param name="triesToConnect">
        /// Number of times to try to connect to database. 
        /// If connection cannot be open, then error will be returned. 
        /// Pass 0 to skip this validation.
        /// </param>
        /// <returns>Error</returns>
        protected virtual string CreateDatabase(string connectionString, string collation, int triesToConnect = 10)
        {
            try
            {
                //parse database name
                var builder = new SqlConnectionStringBuilder(connectionString);
                var databaseName = builder.InitialCatalog;
                //now create connection string to 'master' dabatase. It always exists.
                builder.InitialCatalog = "master";
                var masterCatalogConnectionString = builder.ToString();
                var query = $"CREATE DATABASE [{databaseName}]";
                if (!string.IsNullOrWhiteSpace(collation))
                    query = $"{query} COLLATE {collation}";
                using (var conn = new SqlConnection(masterCatalogConnectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                //try connect
                if (triesToConnect <= 0)
                    return string.Empty;

                //sometimes on slow servers (hosting) there could be situations when database requires some time to be created.
                //but we have already started creation of tables and sample data.
                //as a result there is an exception thrown and the installation process cannot continue.
                //that's why we are in a cycle of "triesToConnect" times trying to connect to a database with a delay of one second.
                for (var i = 0; i <= triesToConnect; i++)
                {
                    if (i == triesToConnect)
                        throw new Exception("Unable to connect to the new database. Please try one more time");

                    if (!SqlServerDatabaseExists(connectionString))
                        Thread.Sleep(1000);
                    else
                        break;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return "DatabaseCreationError";
            }
        }

        private bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //just try to connect
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
