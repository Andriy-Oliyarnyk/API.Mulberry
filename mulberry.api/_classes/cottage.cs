using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace mulberry.api.classes
{
    public class cottage_info
    {
        public long property_code = 0;
        public decimal min_price = 0.0M;
        public decimal property_2_night = 0.0M;
        public decimal property_3_night = 0.0M;
        public decimal property_4_night = 0.0M;
        public string price_override = "";

    }

    public class cottage
    {
        public static cottage_info GetInfo(long property_code, long site_id)
        {
            cottage_info cottage_info = new cottage_info();

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_GetCottageInfo", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@property_code", property_code);
                command.Parameters.AddWithValue("@site_id", site_id);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    cottage_info.property_code = common.NullSafeLong(reader["property_code"]);
                    cottage_info.min_price = common.NullSafeInteger(reader["property_minprice"]);
                    cottage_info.property_2_night = common.NullSafeInteger(reader["property_2_night"]);
                    cottage_info.property_3_night = common.NullSafeInteger(reader["property_3_night"]);
                    cottage_info.property_4_night = common.NullSafeInteger(reader["property_4_night"]);
                    cottage_info.price_override = common.NullSafeString(reader["property_price"]);
                }
            }

            return cottage_info;
        }
    }
}