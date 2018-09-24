using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace mulberry.api.classes
{
    public class api_key_info
    {
        public long site_id = 0;
        public string key = "";
        public int active = 0;
    }

    public class api
    {
        public static api_key_info GetKeyInfo(string api_key)
        {
            api_key_info api_key_info = new api_key_info();

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_GetAPIKeyInfo", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@api_key", api_key.Trim());

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    api_key_info.site_id = common.NullSafeLong(reader["api_site_id"]);
                    api_key_info.key = common.NullSafeString(reader["api_key"]);
                    api_key_info.active = common.NullSafeInteger(reader["api_active"]);
                }
            }

            return api_key_info;
        }

        public static bool ValidateJSON(string json)
        {
            try
            {
                JsonConvert.DeserializeObject(json);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        public static void SaveSearch(long region, long county, long location, string sleeps, string keyword, string start_date, int number_of_nights, string features, string search_user_agent)
        {
            if (sleeps == "")
            {
                sleeps = "0";
            }
            else
            {
                switch (sleeps)
                {
                    case "1-4":
                        sleeps = "1-4";
                        break;

                    case "5-9":
                        sleeps = "5-9";
                        break;

                    case "10-15":
                        sleeps = "10-15";
                        break;

                    case "16":
                        sleeps = "16+";
                        break;

                    case "1":
                        sleeps = "1";
                        break;

                    case "2":
                        sleeps = "2";
                        break;

                    case "3":
                        sleeps = "3";
                        break;

                    case "4":
                        sleeps = "4";
                        break;

                    case "5":
                        sleeps = "5";
                        break;

                    case "6":
                        sleeps = "6";
                        break;

                    case "7":
                        sleeps = "7";
                        break;

                    case "8":
                        sleeps = "8";
                        break;

                    case "9":
                        sleeps = "9";
                        break;

                    case "10":
                        sleeps = "10";
                        break;

                    case "12":
                        sleeps = "12";
                        break;

                    case "14":
                        sleeps = "14";
                        break;

                    case "15":
                        sleeps = "15";
                        break;

                    case "18":
                        sleeps = "18";
                        break;

                    case "20":
                        sleeps = "20";
                        break;

                    case "24":
                        sleeps = "24";
                        break;

                    case "28":
                        sleeps = "28";
                        break;

                    case "30":
                        sleeps = "30+";
                        break;

                    case "40":
                        sleeps = "40+";
                        break;

                    default:
                        sleeps = "0";
                        break;
                }
            }

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spAPI_SaveSearch", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@search_region", region);
                command.Parameters.AddWithValue("@search_county", county);
                command.Parameters.AddWithValue("@search_town", location);
                command.Parameters.AddWithValue("@search_group_size", sleeps);
                command.Parameters.AddWithValue("@search_keyword", keyword);
                command.Parameters.AddWithValue("@search_arrival_date", start_date);
                command.Parameters.AddWithValue("@search_number_nights", number_of_nights);
                command.Parameters.AddWithValue("@search_features", features);
                command.Parameters.AddWithValue("@search_user_agent", search_user_agent);

                command.ExecuteNonQuery();
            }
        }
    }
}