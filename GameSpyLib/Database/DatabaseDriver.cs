using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace GameSpyLib.Database
{
    public abstract class DatabaseDriver : IDisposable
    {
        /// <summary>
        /// Current DB Engine
        /// </summary>
        public DatabaseEngine DatabaseEngine { get; protected set; }

        /// <summary>
        /// The database connection
        /// </summary>
        public DbConnection Connection { get; protected set; }

        /// <summary>
        /// Returns whether the Database connection is open
        /// </summary>
        public bool IsConnected
        {
            get { return (Connection.State == ConnectionState.Open); }
        }

        /// <summary>
        /// Returns the current conenction state of the database
        /// </summary>
        public ConnectionState State
        {
            get { return Connection.State; }
        }

        /// <summary>
        /// Gets the number of queries ran by this instance
        /// </summary>
        public int NumQueries = 0;

        /// <summary>
        /// Random, yes... But its used here when building queries dynamically
        /// </summary>
        protected static char Comma = ',';

        /// <summary>
        /// Indicates whether the disposed method was called
        /// </summary>
        protected bool IsDisposed = false;

        /// <summary>
        /// Destructor
        /// </summary>
        ~DatabaseDriver()
        {
            Dispose();
        }

        /// <summary>
        /// Disposes the DB connection
        /// </summary>
        public void Dispose()
        {
            if (Connection != null && !IsDisposed)
            {
                try
                {
                    Connection.Close();
                    Connection.Dispose();
                }
                catch (ObjectDisposedException) { }

                IsDisposed = true;
            }
        }

        /// <summary>
        /// Opens the database connection
        /// </summary>
        public void Connect()
        {
            if (Connection.State != ConnectionState.Open)
            {
                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    throw new DbConnectException("Unable to establish a database connection.", e);
                }
            }
        }

        /// <summary>
        /// Closes the connection to the database
        /// </summary>
        public void Close()
        {
            try
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Creates a new command to be executed on the database
        /// </summary>
        /// <param name="QueryString"></param>
        public abstract DbCommand CreateCommand(string QueryString);

        /// <summary>
        /// Creates a DbParameter using the current Database engine's Parameter object
        /// </summary>
        /// <returns></returns>
        public abstract DbParameter CreateParam();


        /// <summary>
        /// Queries the database, and returns a result set
        /// </summary>
        /// <param name="Sql">The SQL Statement to run on the database</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Query(string Sql)
        {
            return this.Query(Sql, new List<DbParameter>());
        }

        /// <summary>
        /// Queries the database, and returns a result set
        /// </summary>
        /// <param name="Sql">The SQL Statement to run on the database</param>
        /// <param name="Items">Additional parameters are parameter values for the query.
        /// The first parameter replaces @P0, second @P1 etc etc.
        /// </param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Query(string Sql, params object[] Items)
        {
            List<DbParameter> Params = new List<DbParameter>(Items.Length);
            for (int i = 0; i < Items.Length; i++)
            {
                DbParameter Param = this.CreateParam();
                Param.ParameterName = "@P" + i;
                Param.Value = Items[i];
                Params.Add(Param);
            }

            return this.Query(Sql, Params);
        }

        /// <summary>
        /// Queries the database, and returns a result set
        /// </summary>
        /// <param name="Sql">The SQL Statement to run on the database</param>
        /// <param name="Params">A list of sql params to add to the command</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> Query(string Sql, List<DbParameter> Params)
        {
            // Create our Rows result
            List<Dictionary<string, object>> Rows = new List<Dictionary<string, object>>();

            // Increase Query Count
            NumQueries++;

            // Create the SQL Command
            using (DbCommand Command = this.CreateCommand(Sql))
            {
                // Add params
                foreach (DbParameter Param in Params)
                    Command.Parameters.Add(Param);

                // Execute the query
                using (DbDataReader Reader = Command.ExecuteReader())
                {
                    // If we have rows, add them to the list
                    if (Reader.HasRows)
                    {
                        // Add each row to the rows list
                        while (Reader.Read())
                        {
                            NiceDictionary<string, object> Row = new NiceDictionary<string, object>(Reader.FieldCount);
                            for (int i = 0; i < Reader.FieldCount; ++i)
                                Row.Add(Reader.GetName(i), Reader.GetValue(i));

                            Rows.Add(Row);
                        }
                    }

                    // Cleanup
                    Reader.Close();
                }
            }

            // Return Rows
            return Rows;
        }

        /// <summary>
        /// Queries the database, and returns 1 row at a time until all rows are returned
        /// </summary>
        /// <param name="Sql">The SQL Statement to run on the database</param>
        /// <returns></returns>
        public IEnumerable<Dictionary<string, object>> QueryReader(string Sql)
        {
            // Increase Query Count
            NumQueries++;

            // Create the SQL Command, and execute the reader
            using (DbCommand Command = this.CreateCommand(Sql))
            using (DbDataReader Reader = Command.ExecuteReader())
            {
                // If we have rows, add them to the list
                if (Reader.HasRows)
                {
                    // Add each row to the rows list
                    while (Reader.Read())
                    {
                        Dictionary<string, object> Row = new Dictionary<string, object>(Reader.FieldCount);
                        for (int i = 0; i < Reader.FieldCount; ++i)
                            Row.Add(Reader.GetName(i), Reader.GetValue(i));

                        yield return Row;
                    }
                }

                // Cleanup
                Reader.Close();
            }
        }

        /// <summary>
        /// Executes a command, and returns 1 row at a time until all rows are returned
        /// </summary>
        /// <param name="Command">The database command to execute the reader on</param>
        /// <returns></returns>
        public IEnumerable<Dictionary<string, object>> QueryReader(DbCommand Command)
        {
            // Increase Query Count
            NumQueries++;

            // Execute the query
            using (Command)
            using (DbDataReader Reader = Command.ExecuteReader())
            {
                // If we have rows, add them to the list
                if (Reader.HasRows)
                {
                    // Add each row to the rows list
                    while (Reader.Read())
                    {
                        Dictionary<string, object> Row = new Dictionary<string, object>(Reader.FieldCount);
                        for (int i = 0; i < Reader.FieldCount; ++i)
                            Row.Add(Reader.GetName(i), Reader.GetValue(i));

                        yield return Row;
                    }
                }

                // Cleanup
                Reader.Close();
            }
        }


        /// <summary>
        /// Executes a command, and returns the resulting rows
        /// </summary>
        /// <param name="Command">The database command to execute the reader on</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> ExecuteReader(DbCommand Command)
        {
            // Execute the query
            List<Dictionary<string, object>> Rows = new List<Dictionary<string, object>>();

            // Increase Query Count
            NumQueries++;

            using (Command)
            using (DbDataReader Reader = Command.ExecuteReader())
            {
                // If we have rows, add them to the list
                if (Reader.HasRows)
                {
                    // Add each row to the rows list
                    while (Reader.Read())
                    {
                        Dictionary<string, object> Row = new Dictionary<string, object>(Reader.FieldCount);
                        for (int i = 0; i < Reader.FieldCount; ++i)
                            Row.Add(Reader.GetName(i), Reader.GetValue(i));

                        Rows.Add(Row);
                    }
                }

                // Cleanup
                Reader.Close();
            }

            // Return Rows
            return Rows;
        }

        /// <summary>
        /// Executes a statement on the database (Update, Delete, Insert)
        /// </summary>
        /// <param name="Sql">The SQL statement to be executes</param>
        /// <returns>Returns the number of rows affected by the statement</returns>
        public int Execute(string Sql)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
                return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes a statement on the database (Update, Delete, Insert)
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        /// <param name="Params">A list of Sqlparameters</param>
        /// <returns>Returns the number of rows affected by the statement</returns>
        public int Execute(string Sql, List<DbParameter> Params)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
            {
                // Increase Query Count
                NumQueries++;

                // Add params
                foreach (DbParameter Param in Params)
                    Command.Parameters.Add(Param);

                // Execute command, and dispose of the command
                return Command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a statement on the database (Update, Delete, Insert)
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        /// <param name="Items">Additional parameters are parameter values for the query.
        /// The first parameter replaces @P0, second @P1 etc etc.
        /// </param>
        /// <returns>Returns the number of rows affected by the statement</returns>
        public int Execute(string Sql, params object[] Items)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
            {
                // Add params
                for (int i = 0; i < Items.Length; i++)
                {
                    DbParameter Param = CreateParam();
                    Param.ParameterName = "@P" + i;
                    Param.Value = Items[i];
                    Command.Parameters.Add(Param);
                }

                // Increase Query Count
                NumQueries++;

                // Execute command, and dispose of the command
                return Command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result 
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        /// <returns></returns>
        public object ExecuteScalar(string Sql)
        {
            // Increase Query Count
            NumQueries++;

            // Create the SQL Command
            using (DbCommand Command = this.CreateCommand(Sql))
                return Command.ExecuteScalar();
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result 
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        /// <param name="Params">A list of Sqlparameters</param>
        /// <returns></returns>
        public object ExecuteScalar(string Sql, List<DbParameter> Params)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
            {
                // Increase Query Count
                NumQueries++;

                // Add params
                foreach (DbParameter Param in Params)
                    Command.Parameters.Add(Param);

                // Execute command, and dispose of the command
                return Command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result 
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        /// <param name="Items"></param>
        /// <returns></returns>
        public object ExecuteScalar(string Sql, params object[] Items)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
            {
                // Add params
                for (int i = 0; i < Items.Length; i++)
                {
                    DbParameter Param = CreateParam();
                    Param.ParameterName = "@P" + i;
                    Param.Value = Items[i];
                    Command.Parameters.Add(Param);
                }

                // Increase Query Count
                NumQueries++;

                // Execute command, and dispose of the command
                return Command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result 
        /// set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="Sql">The SQL statement to be executed</param>
        public T ExecuteScalar<T>(string Sql, params object[] Items)
        {
            // Create the SQL Command
            using (DbCommand Command = CreateCommand(Sql))
            {
                // Add params
                for (int i = 0; i < Items.Length; i++)
                {
                    DbParameter Param = CreateParam();
                    Param.ParameterName = "@P" + i;
                    Param.Value = Items[i];
                    Command.Parameters.Add(Param);
                }

                // Increase Query Count
                NumQueries++;

                // Execute command, and dispose of the command
                object Value = Command.ExecuteScalar();
                return (T)Convert.ChangeType(Value, typeof(T), CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return Connection.BeginTransaction();
        }

        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        /// <param name="Level"></param>
        /// <returns></returns>
        public DbTransaction BeginTransaction(IsolationLevel Level)
        {
            return Connection.BeginTransaction(Level);
        }

        /// <summary>
        /// Converts a database string name to a DatabaseEngine type.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>The current database engine</returns>
        public static DatabaseEngine GetDatabaseEngine(string Name)
        {
            return ((DatabaseEngine)Enum.Parse(typeof(DatabaseEngine), Name, true));
        }
    }
}
