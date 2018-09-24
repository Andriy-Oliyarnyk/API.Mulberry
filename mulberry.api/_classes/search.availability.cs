using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace mulberry.api.classes
{
    public class search_availability
    {
        public class Availability
        {
            public int PropertyAllocatable { get; set; }
            public int PropertyReservable { get; set; }
            public int PropertyReserved { get; set; }
        }

        // forming request availability cottage on id
        public static string FormingRequestAvailabilityCottage(int property_resource_id)
        {
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT * FROM tblMaxx_PropertyAvailability WHERE property_resource_id = " + property_resource_id);
            return sql_query.ToString();
        }

        // search availability cottage on id 
        public static Availability SearchResourceIdAvailability(int property_resource_id)
        {
            Availability availability = new Availability();
            string sql_query = FormingRequestAvailabilityCottage(property_resource_id);
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
                        availability.PropertyAllocatable = classes.common.NullSafeInteger(reader["allocatable"]);
                        availability.PropertyReservable = classes.common.NullSafeInteger(reader["reservable"]);
                        availability.PropertyReserved = classes.common.NullSafeInteger(reader["reserved"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return availability;
        }
    }
}