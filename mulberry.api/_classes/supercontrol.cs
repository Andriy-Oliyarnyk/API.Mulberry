using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace mulberry.api.classes
{
    public class supercontrol
    {
        public static bool IsDown()
        {
            bool down = false;
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT TOP 1 mode");
            sql_query.AppendLine("FROM tblSCMode");

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query.ToString(), connection);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    if (common.NullSafeInteger(reader["mode"]) == 1)
                    {
                        down = true;
                    }
                }
            }

            return down;
        }

        public static void StoreError(int error_type, string error_message, string error_page)
        {
            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_StoreSCError", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@error_type", error_type);
                command.Parameters.AddWithValue("@error_message", error_message);
                command.Parameters.AddWithValue("@error_page", error_page);

                command.ExecuteNonQuery();
            }
        }
    }
}