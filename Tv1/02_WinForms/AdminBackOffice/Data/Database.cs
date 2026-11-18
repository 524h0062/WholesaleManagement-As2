using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AdminBackOffice.Data
{
    public static class Database
    {
        public static SqlConnection Open()
        {
            var connectionSetting = ConfigurationManager.ConnectionStrings["WholesaleDB"];
            if (connectionSetting == null || string.IsNullOrWhiteSpace(connectionSetting.ConnectionString))
                throw new InvalidOperationException("Missing WholesaleDB connection string in App.config.");
            var connection = new SqlConnection(connectionSetting.ConnectionString);
            connection.Open();
            return connection;
        }

        public static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Open())
            using (var command = new SqlCommand(sql, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                command.CommandTimeout = 20;
                if (parameters != null) command.Parameters.AddRange(parameters);
                var data = new DataTable();
                adapter.Fill(data);
                return data;
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Open())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandTimeout = 20;
                if (parameters != null) command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Open())
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandTimeout = 20;
                if (parameters != null) command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
        }
    }
}
