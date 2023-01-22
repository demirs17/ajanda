using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Ajanda
{
    class Sorgu
    {
        public static SqlConnection conn;
        public static SqlCommand query(string sql)
        {
            string connStr = "Server=localhost;Database=Ajanda;Trusted_Connection=True";
            conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand command;
            command = new SqlCommand(sql, conn);

            return command;
        }
        public static SqlDataReader select(string sql)
        {
            string connStr = "Server=localhost;Database=Ajanda;Trusted_Connection=True";
            conn = new SqlConnection(connStr);
            conn.Open();

            SqlCommand command;
            SqlDataReader reader;

            command = new SqlCommand(sql, conn);
            reader = command.ExecuteReader();

            return reader;
        }
    }
}
