using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mulberry.api.classes
{
    public static class search_details
    {
        public class Details
        {
            public int PropertySleeps { get; set; }
            public int PropertySleepsAdults { get; set; }
            public int PropertySleepsChildren { get; set; }
            public int PropertySleepsInfants { get; set; }
            public int PropertyPets { get; set; }
        }

        // forming request details cottage on id
        public static string FormingRequestDetailsCottage(int property_resource_id)
        {
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT * FROM tblSC_TempPropertyDetails WHERE property_resource_id = " + property_resource_id);
            return sql_query.ToString();
        }

        // search details cottage on id
        public static Details SearchResourceIdDetails(int property_resource_id)
        {
            Details details = new Details();
            string sql_query = FormingRequestDetailsCottage(property_resource_id);
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
                        details.PropertySleeps = classes.common.NullSafeInteger(reader["property_sleeps"]);
                        details.PropertySleepsAdults = classes.common.NullSafeInteger(reader["property_sleeps_adult"]);
                        details.PropertySleepsChildren = classes.common.NullSafeInteger(reader["property_sleeps_children"]);
                        details.PropertySleepsInfants = classes.common.NullSafeInteger(reader["property_sleeps_infants"]);
                        details.PropertyPets = classes.common.NullSafeInteger(reader["property_pets"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return details;
        }
    }
}