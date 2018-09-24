using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace mulberry.api.classes
{
    public class cms_offer_category_info
    {
        public long id = 0;
        public string title = "";
        public string subtitle = "";
        public string description = "";
        public string header_image = "";
        public string box_colour = "";
        public int selection = 0;
        public string search_region_county = "";
        public int search_town = 0;
        public string search_sleeps = "";
        public int search_bedrooms = 0;
        public string search_variable = "";
        public object date_start;
        public object date_end;
        public string[] properties;
        public long[] features;
    }

    public class cms_offer_category
    {
        public static cms_offer_category_info GetInfo(long offer_category_id)
        {
            cms_offer_category_info cms_offer_category_info = new cms_offer_category_info();

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_CMSGetOfferCategoryInfo", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@offer_category_id", offer_category_id);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cms_offer_category_info.id = common.NullSafeLong(reader["offer_category_id"]);
                    cms_offer_category_info.title = common.NullSafeString(reader["offer_category_title"]);
                    cms_offer_category_info.subtitle = common.NullSafeString(reader["offer_category_subtitle"]);
                    cms_offer_category_info.description = common.NullSafeString(reader["offer_category_description"]);
                    cms_offer_category_info.header_image = common.NullSafeString(reader["offer_category_header_image"]);
                    cms_offer_category_info.box_colour = common.NullSafeString(reader["offer_category_box_colour"]);
                    cms_offer_category_info.selection = common.NullSafeInteger(reader["offer_category_selection"]);
                    cms_offer_category_info.search_region_county = common.NullSafeString(reader["offer_category_search_region_county"]);
                    cms_offer_category_info.search_town = common.NullSafeInteger(reader["offer_category_search_town"]);
                    cms_offer_category_info.search_sleeps = common.NullSafeString(reader["offer_category_search_sleeps"]);
                    cms_offer_category_info.search_bedrooms = common.NullSafeInteger(reader["offer_category_search_bedrooms"]);
                    cms_offer_category_info.search_variable = common.NullSafeString(reader["offer_category_search_variable"]);
                    cms_offer_category_info.date_start = reader["offer_category_date_start"];
                    cms_offer_category_info.date_end = reader["offer_category_date_end"];
                    cms_offer_category_info.properties = GetProperties(offer_category_id, 1);
                    cms_offer_category_info.features = GetFeatures(offer_category_id, 2);
                }
            }

            return cms_offer_category_info;
        }

        private static string[] GetProperties(long offer_category_id, int selection_type)
        {
            System.Text.StringBuilder content = new System.Text.StringBuilder();

            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_CMSGetOfferCategoriesSelection", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@offer_category_id", offer_category_id);
                command.Parameters.AddWithValue("@selection_type", selection_type);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long selection_id = classes.common.NullSafeInteger(reader["selection_id"]);

                    content.Append(selection_id + ",");
                }
            }

            string[] sections = content.ToString().TrimEnd(',').Split(',');

            return sections;
        }

        private static long[] GetFeatures(long offer_category_id, int selection_type)
        {
            System.Text.StringBuilder content = new System.Text.StringBuilder();

            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_CMSGetOfferCategoriesSelection", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@offer_category_id", offer_category_id);
                command.Parameters.AddWithValue("@selection_type", selection_type);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long selection_id = classes.common.NullSafeInteger(reader["selection_id"]);

                    content.Append(selection_id + ",");
                }
            }

            long[] sections = { 0 };
            if (content.ToString() != "")
            {
                sections = Array.ConvertAll<string, long>(content.ToString().TrimEnd(',').Split(','), long.Parse);
            }

            return sections;
        }
    }
}