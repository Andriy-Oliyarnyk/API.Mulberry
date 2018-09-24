using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mulberry.api.classes
{
    public class search_rentabilities
    {
        public class Rentabilities
        {
            public string PropertyType { get; set; }
            public string PropertyDayOfWeek { get; set; }
            public DateTime PropertyBookDateFrom { get; set; }
            public DateTime PropertyBookDateTo { get; set; }
            public int PropertyMinDuration { get; set; }
            public int PropertyMaxDuration { get; set; }
        }

        // forming request rentabilities cottage on id 
        public static string FormingRequestRentabilitiesCottage(int property_resource_id)
        {
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT * FROM tblMaxx_PropertyRentabilities WHERE property_resource_id = " + property_resource_id);
            return sql_query.ToString();
        }

        // search rentabilities cottage on id 
        public static Rentabilities SearchResourceIdRentabilities(int property_resource_id)
        {
            Rentabilities rentabilities = new Rentabilities();
            string sql_query = FormingRequestRentabilitiesCottage(property_resource_id);
            System.Text.StringBuilder content = new System.Text.StringBuilder();
            try
            {
                using (SqlConnection connection = new SqlConnection(classes.common.NewConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql_query, connection);
                    command.CommandType = System.Data.CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        rentabilities.PropertyType = classes.common.NullSafeString(reader["type"]);
                        rentabilities.PropertyDayOfWeek = classes.common.NullSafeString(reader["dayOfWeek"]);
                        rentabilities.PropertyBookDateFrom = classes.common.NullSafeDate(reader["bookDateFrom"]);
                        rentabilities.PropertyBookDateTo = classes.common.NullSafeDate(reader["bookDateTo"]);
                        rentabilities.PropertyMinDuration = classes.common.NullSafeInteger(reader["minDuration"]);
                        rentabilities.PropertyMaxDuration = classes.common.NullSafeInteger(reader["maxDuration"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return rentabilities;
        }
    }
}