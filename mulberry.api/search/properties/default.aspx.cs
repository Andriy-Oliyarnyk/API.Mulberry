using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace mulberry.api.search.properties
{
    public class Api
    {
        public string key { get; set; }
        public bool test_mode { get; set; }
    }

    public class Search
    {
        public string mode { get; set; }
        public string property { get; set; }
        public string region { get; set; }
        public long town { get; set; }
        public string arrival_date { get; set; }
        public string departure_date { get; set; }
        public string sleeps { get; set; }
        public int bedrooms { get; set; }
        public int min_price { get; set; }
        public int max_price { get; set; }
        public int flexibility { get; set; }
        public string features { get; set; }
        public string variables { get; set; }
        public int late_availability_days { get; set; }
        public int late_availability_days_count { get; set; }
        public string offer_cutoff { get; set; }
        public long offer_category_id { get; set; }
        public string order_by { get; set; }
        public int properties_per_page { get; set; }
        public int page { get; set; }
    }

    public class RootObject
    {
        public Api api { get; set; }
        public Search search { get; set; }
    }

    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //clean function finish correct program
            mulberry.api._classes.PropertyCalendar.GetPropertiesCalendar(4023277);



            System.Text.StringBuilder json_content = new System.Text.StringBuilder();
            string postData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();

            //json_content.AppendLine("{");
            //json_content.AppendLine("   \"api\":{");
            //json_content.AppendLine("       \"key\": \"78g878agda78sdgad98abd\"");
            //json_content.AppendLine("   },");
            //json_content.AppendLine("   \"search\":{");
            //json_content.AppendLine("       \"mode\": \"\",");
            //json_content.AppendLine("       \"property\": \"\",");
            //json_content.AppendLine("       \"region\": \"c_1113\",");
            //json_content.AppendLine("       \"town\": 247,");
            //json_content.AppendLine("       \"arrival_date\": \"08/02/2017\",");
            //json_content.AppendLine("       \"departure_date\": \"14/02/2017\",");
            //json_content.AppendLine("       \"sleeps\": 3,");
            //json_content.AppendLine("       \"bedrooms\": 2,");
            //json_content.AppendLine("       \"min_price\": 0,");
            //json_content.AppendLine("       \"max_price\": 0,");
            //json_content.AppendLine("       \"flexibility\": 0,");
            //json_content.AppendLine("       \"features\": \"\",");
            //json_content.AppendLine("       \"order_by\": \"POPULARITY\",");
            //json_content.AppendLine("       \"properties_per_page\": 25,");
            //json_content.AppendLine("       \"page\": 1");
            //json_content.AppendLine("   }");
            //json_content.AppendLine("}");
            //postData = json_content.ToString();


            if (postData != "")
            {
                if (classes.api.ValidateJSON(postData))
                {
                    var json = JsonConvert.DeserializeObject<RootObject>(postData);

                    classes.api_key_info api_key_info = classes.api.GetKeyInfo(json.api.key);
                    string[] errors = new string[8];
                    int err = 0;
                    int i = 0;

                    if (api_key_info.key == "")
                    {
                        errors[0] = "API Key does not exist";
                        err = 1;
                    }
                    else
                    {
                        if (api_key_info.active == 0)
                        {
                            errors[0] = "API Key is no longer active";
                            err = 1;
                        }
                    }

                    //if (json.search.arrival_date != "")
                    //{
                    //    if (!Regex.IsMatch(json.search.arrival_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)"))
                    //    {
                    //        errors[1] = "arrival_date must be in this format DD/MM/YYYY";
                    //        err = 1;
                    //    }
                    //    else
                    //    {
                    //        if (classes.common.NullSafeString(json.search.mode) != "LASTMINUTE" && classes.common.NullSafeString(json.search.mode) != "LATEAVAILABILITY")
                    //        {
                    //            if (classes.common.NullSafeDate(json.search.arrival_date) < classes.common.NullSafeDate(DateTime.Now.ToString("dd/MM/yyyy")))
                    //            {
                    //                errors[1] = "arrival_date cannot be in the past";
                    //                err = 1;
                    //            }
                    //        }
                    //    }
                    //}

                    //if (json.search.departure_date != "")
                    //{
                    //    if (!Regex.IsMatch(json.search.departure_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)"))
                    //    {
                    //        errors[2] = "departure_date must be in this format DD/MM/YYYY";
                    //        err = 1;
                    //    }
                    //    else
                    //    {
                    //        if (classes.common.NullSafeDate(json.search.departure_date) < classes.common.NullSafeDate(json.search.arrival_date))
                    //        {
                    //            errors[2] = "departure_date cannot be less than arrival_date";
                    //            err = 1;
                    //        }
                    //    }
                    //}

                    //if (classes.common.NullSafeString(json.search.mode) != "LASTMINUTE" && classes.common.NullSafeString(json.search.mode) != "LATEAVAILABILITY")
                    //{
                    //    if (json.search.arrival_date != "" || json.search.departure_date != "")
                    //    {
                    //        if (json.search.arrival_date == "")
                    //        {
                    //            errors[3] = "arrival_date cannot be empty if departure_date has data";
                    //            err = 1;
                    //        }

                    //        if (json.search.departure_date == "")
                    //        {
                    //            errors[4] = "departure_date cannot be empty if arrival_date has data";
                    //            err = 1;
                    //        }
                    //    }
                    //}

                    if (json.search.order_by == "")
                    {
                        errors[5] = "order_by must contain a value";
                        err = 1;
                    }

                    if (json.search.properties_per_page <= 0)
                    {
                        errors[6] = "properties_per_page must contain a value larger than 0";
                        err = 1;
                    }

                    if (json.search.page == 0)
                    {
                        errors[7] = "page must contain a value larger than 0 or -1 (All)";
                        err = 1;
                    }


                    if (err == 1)
                    {
                        System.Text.StringBuilder error_text = new System.Text.StringBuilder();
                        for (i = 0; i < errors.Length; i++)
                        {
                            if (errors[i] != null)
                            {
                                error_text.Append("\"" + errors[i] + "\",");
                            }
                        }

                        json_content.Append("{");
                        json_content.Append("   \"api\":{");
                        json_content.Append("       \"success\": false,");
                        json_content.Append("       \"messages\": [");
                        json_content.Append(error_text.ToString().TrimEnd(','));
                        json_content.Append("       ]");
                        json_content.Append("   }");
                        json_content.Append("}");
                    }
                    else
                    {
                        if (json.search.page == -1)
                        {
                            json.search.page = 1;
                            json.search.properties_per_page = 9999;
                        }

                        try
                        {
                            System.Text.StringBuilder additional_message = new System.Text.StringBuilder();
                            bool is_supercontrol_down = classes.supercontrol.IsDown();
                            if (is_supercontrol_down == true)
                            {
                                additional_message.Append("\"Availability searches are currently offline, please try again later\",");
                                json.search.arrival_date = "";
                                json.search.departure_date = "";
                            }

                            // If dates are not valid dates reset to non-date search
                            if (json.search.arrival_date != "" && json.search.departure_date != "")
                            {
                                if (!Regex.IsMatch(json.search.arrival_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)") || !Regex.IsMatch(json.search.departure_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)"))
                                {
                                    json.search.arrival_date = "";
                                    json.search.departure_date = "";
                                }
                            }

                            int nights = 0;
                            if ((json.search.arrival_date != "" && json.search.departure_date != "") || (json.search.mode == "LASTMINUTE" || json.search.mode == "LATEAVAILABILITY"))
                            {
                                DateTime date1 = Convert.ToDateTime(json.search.arrival_date);
                                DateTime date2 = Convert.ToDateTime(json.search.departure_date);

                                TimeSpan nights_calc = date2 - date1;
                                nights = classes.common.NullSafeInteger(nights_calc.Days);
                            }

                            string[] str = classes.search_property.ResultsPropertyCodes(api_key_info.site_id, classes.common.NullSafeString(json.search.mode), is_supercontrol_down, classes.common.NullSafeString(json.search.region), classes.common.NullSafeLong(json.search.town), classes.common.NullSafeString(json.search.arrival_date), classes.common.NullSafeString(json.search.departure_date), classes.common.NullSafeString(json.search.sleeps), classes.common.NullSafeInteger(json.search.bedrooms), classes.common.NullSafeInteger(json.search.min_price), classes.common.NullSafeInteger(json.search.max_price), classes.common.NullSafeInteger(json.search.flexibility), classes.common.NullSafeString(json.search.features), classes.common.NullSafeString(json.search.variables), classes.common.NullSafeInteger(json.search.late_availability_days), classes.common.NullSafeInteger(json.search.late_availability_days_count), classes.common.NullSafeString(json.search.offer_cutoff), classes.common.NullSafeLong(json.search.offer_category_id), classes.common.NullSafeString(json.search.property));
                            string results_property_codes = str[0];
                            string property_codes = str[1];
                            string search_sql = classes.search_property.Run(api_key_info.site_id, classes.common.NullSafeString(json.search.mode), is_supercontrol_down, classes.common.NullSafeString(json.search.region), classes.common.NullSafeLong(json.search.town), classes.common.NullSafeString(json.search.arrival_date), classes.common.NullSafeString(json.search.departure_date), classes.common.NullSafeString(json.search.sleeps), classes.common.NullSafeInteger(json.search.bedrooms), classes.common.NullSafeInteger(json.search.min_price), classes.common.NullSafeInteger(json.search.max_price), classes.common.NullSafeInteger(json.search.flexibility), classes.common.NullSafeString(json.search.features), classes.common.NullSafeString(json.search.variables), classes.common.NullSafeInteger(json.search.late_availability_days), classes.common.NullSafeInteger(json.search.late_availability_days_count), classes.common.NullSafeString(json.search.offer_cutoff), classes.common.NullSafeLong(json.search.offer_category_id), classes.common.NullSafeString(json.search.property), 0, results_property_codes, property_codes);
                            string search_sql_count = classes.search_property.Run(api_key_info.site_id, classes.common.NullSafeString(json.search.mode), is_supercontrol_down, classes.common.NullSafeString(json.search.region), classes.common.NullSafeLong(json.search.town), classes.common.NullSafeString(json.search.arrival_date), classes.common.NullSafeString(json.search.departure_date), classes.common.NullSafeString(json.search.sleeps), classes.common.NullSafeInteger(json.search.bedrooms), classes.common.NullSafeInteger(json.search.min_price), classes.common.NullSafeInteger(json.search.max_price), classes.common.NullSafeInteger(json.search.flexibility), classes.common.NullSafeString(json.search.features), classes.common.NullSafeString(json.search.variables), classes.common.NullSafeInteger(json.search.late_availability_days), classes.common.NullSafeInteger(json.search.late_availability_days_count), classes.common.NullSafeString(json.search.offer_cutoff), classes.common.NullSafeLong(json.search.offer_category_id), classes.common.NullSafeString(json.search.property), 1, results_property_codes, property_codes);

                            long record_count = classes.search_property.Count(search_sql_count);
                            double pages_split = (record_count / json.search.properties_per_page) + 0.5;
                            int pages_needed = classes.common.NullSafeInteger(Math.Ceiling(pages_split));

                            string sql_order = "";
                            switch (json.search.order_by)
                            {
                                case "PRICE_ASC":
                                    sql_order = "ORDER BY property_price_order ASC";
                                    break;

                                case "PRICE_DESC":
                                    sql_order = "ORDER BY property_price_order DESC";
                                    break;

                                case "SLEEPS_ASC":
                                    sql_order = "ORDER BY property_sleeps ASC";
                                    break;

                                case "SLEEPS_DESC":
                                    sql_order = "ORDER BY property_sleeps DESC";
                                    break;

                                case "POPULARITY":
                                    sql_order = "ORDER BY property_order DESC";
                                    break;

                                default:
                                    sql_order = "ORDER BY property_order DESC";
                                    break;
                            }

                            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
                            sql_query.AppendLine("DECLARE @FirstRec int, @LastRec int");
                            sql_query.AppendLine("SELECT  @FirstRec = (" + json.search.page + "  - 1) * " + json.search.properties_per_page);
                            sql_query.AppendLine("SELECT  @LastRec  = (" + json.search.page + " * " + json.search.properties_per_page + ");");
                            sql_query.AppendLine("");
                            sql_query.AppendLine("");
                            sql_query.AppendLine("SELECT *");
                            sql_query.AppendLine("FROM (");
                            sql_query.AppendLine("SELECT ROW_NUMBER() OVER (" + sql_order + ") AS Row, property_code, property_order, property_name, property_town, property_postcode, property_sleeps, property_sleeps_text, property_bedrooms, property_bathrooms, property_search_description, property_minprice, property_availability_link, property_lat, property_long, property_1_night, property_2_night, property_3_night, property_4_night, property_5_night, property_6_night, property_deal_text, property_deal_date_start, property_deal_date_end, property_price_order, region_name, property_price, property_booking, thumb_image, is_new, featured, feefo_average, pet_friendly, offer_text");
                            sql_query.AppendLine("FROM (");
                            sql_query.AppendLine(search_sql.Replace("ORDER BY property_order DESC", ""));
                            sql_query.AppendLine(") AS SEARCH_RESULTS");
                            sql_query.AppendLine(") AS SEARCH_PAGING");
                            sql_query.AppendLine("WHERE Row > @FirstRec AND Row <= @LastRec");
                            sql_query.AppendLine(sql_order);

                            //Response.Write(sql_query.ToString());
                            //Response.End();

                            System.Text.StringBuilder results_builder = new System.Text.StringBuilder();
                            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand(sql_query.ToString(), connection);
                                command.CommandType = System.Data.CommandType.Text;

                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    long property_code = classes.common.NullSafeLong(reader["property_code"]);
                                    string property_name = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_name"]));
                                    string property_town = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_town"]));
                                    string property_postcode = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_postcode"]));
                                    int property_sleeps = classes.common.NullSafeInteger(reader["property_sleeps"]);
                                    string property_sleeps_text = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_sleeps_text"]));
                                    int property_bedrooms = classes.common.NullSafeInteger(reader["property_bedrooms"]);
                                    int property_bathrooms = classes.common.NullSafeInteger(reader["property_bathrooms"]);
                                    string property_search_description = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_search_description"]).Replace(Environment.NewLine, ""));
                                    int property_minprice = classes.common.NullSafeInteger(reader["property_minprice"]);
                                    string property_availability_link = classes.common.NullSafeString(reader["property_availability_link"]);
                                    string property_lat = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_lat"]));
                                    string property_long = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_long"]));
                                    int price_one_night = classes.common.NullSafeInteger(reader["property_1_night"]);
                                    int price_two_night = classes.common.NullSafeInteger(reader["property_2_night"]);
                                    int price_three_night = classes.common.NullSafeInteger(reader["property_3_night"]);
                                    int price_four_night = classes.common.NullSafeInteger(reader["property_4_night"]);
                                    int price_five_night = classes.common.NullSafeInteger(reader["property_5_night"]);
                                    int price_six_night = classes.common.NullSafeInteger(reader["property_6_night"]);
                                    string price_overide = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_price"]));
                                    int property_booking = classes.common.NullSafeInteger(reader["property_booking"]);
                                    string property_county = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["region_name"]));
                                    string property_image = classes.common.NullSafeString(reader["thumb_image"]);
                                    string property_deal_text = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["property_deal_text"]));
                                    string property_deal_date_start = (classes.common.NullSafeString(reader["property_deal_date_start"]) != "" ? HttpUtility.HtmlEncode(classes.common.NullSafeDate(reader["property_deal_date_start"]).ToString("dd/MM/yyyy")) : "");
                                    string property_deal_date_end = (classes.common.NullSafeString(reader["property_deal_date_end"]) != "" ? HttpUtility.HtmlEncode(classes.common.NullSafeDate(reader["property_deal_date_end"]).ToString("dd/MM/yyyy")) : "");
                                    int is_new = classes.common.NullSafeInteger(reader["is_new"]);
                                    int is_featured = classes.common.NullSafeInteger(reader["featured"]);
                                    int feefo_average = classes.common.NullSafeInteger(reader["feefo_average"]);
                                    int pet_friendly = classes.common.NullSafeInteger(reader["pet_friendly"]);
                                    string offer_text = classes.common.NullSafeString(reader["offer_text"]);


                                    results_builder.Append("{");
                                    results_builder.Append("  \"property_code\": \"" + property_code + "\",");
                                    results_builder.Append("  \"property_name\": \"" + property_name + "\",");
                                    results_builder.Append("  \"town\": \"" + property_town + "\",");
                                    results_builder.Append("  \"county\": \"" + property_county + "\",");
                                    results_builder.Append("  \"postcode\": \"" + property_postcode + "\",");
                                    results_builder.Append("  \"sleeps\": " + property_sleeps + ",");
                                    results_builder.Append("  \"sleeps_text\": \"" + property_sleeps_text + "\",");
                                    results_builder.Append("  \"bedrooms\": " + property_bedrooms + ",");
                                    results_builder.Append("  \"bathrooms\": " + property_bathrooms + ",");
                                    results_builder.Append("  \"description\": \"" + property_search_description + "\",");
                                    results_builder.Append("  \"min_price\": " + property_minprice + ",");
                                    results_builder.Append("  \"price_one_night\": " + price_one_night + ",");
                                    results_builder.Append("  \"price_two_night\": " + price_two_night + ",");
                                    results_builder.Append("  \"price_three_night\": " + price_three_night + ",");
                                    results_builder.Append("  \"price_four_night\": " + price_four_night + ",");
                                    results_builder.Append("  \"price_five_night\": " + price_five_night + ",");
                                    results_builder.Append("  \"price_six_night\": " + price_six_night + ",");
                                    results_builder.Append("  \"price_overide\": \"" + price_overide + "\",");
                                    results_builder.Append("  \"deal_text\": \"" + property_deal_text + "\",");
                                    results_builder.Append("  \"deal_date_end\": \"" + property_deal_date_end + "\",");

                                    if (offer_text != "")
                                    {
                                        if (json.search.arrival_date != "" && json.search.departure_date != "")
                                        {
                                            if (offer_text.Contains("%"))
                                            {
                                                if (offer_text.Contains(":"))
                                                {
                                                    results_builder.Append("  \"deal_alert\": \"" + offer_text + "\",");
                                                }
                                                else
                                                {
                                                    results_builder.Append("  \"deal_alert\": \"Save " + offer_text + "\",");
                                                }
                                            }
                                            else
                                            {
                                                results_builder.Append("  \"deal_alert\": \"" + offer_text + "\",");
                                            }
                                        }
                                        else
                                        {
                                            if (offer_text.Contains("%"))
                                            {
                                                if (offer_text.Contains(":"))
                                                {
                                                    results_builder.Append("  \"deal_alert\": \"" + offer_text + "\",");
                                                }
                                                else
                                                {
                                                    results_builder.Append("  \"deal_alert\": \"Save up to " + offer_text + "\",");
                                                }
                                            }
                                            else
                                            {
                                                results_builder.Append("  \"deal_alert\": \"" + offer_text + "\",");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        results_builder.Append("  \"deal_alert\": \"\",");
                                    }

                                    results_builder.Append("  \"availability_link\": \"" + property_availability_link + "\",");
                                    results_builder.Append("  \"image\": \"" + property_image + "\",");
                                    results_builder.Append("  \"latitude\": \"" + property_lat + "\",");
                                    results_builder.Append("  \"longitude\": \"" + property_long + "\",");

                                    if (is_new == 1)
                                    {
                                        results_builder.Append("  \"is_new\": true,");
                                    }
                                    else
                                    {
                                        results_builder.Append("  \"is_new\": false,");
                                    }

                                    if (is_featured == 1)
                                    {
                                        results_builder.Append("  \"is_featured\": true,");
                                    }
                                    else
                                    {
                                        results_builder.Append("  \"is_featured\": false,");
                                    }

                                    if (property_booking == 0)
                                    {
                                        results_builder.Append("  \"is_booking_enabled\": true,");
                                    }
                                    else
                                    {
                                        results_builder.Append("  \"is_booking_enabled\": false,");
                                    }

                                    if (pet_friendly == 1)
                                    {
                                        results_builder.Append("  \"is_pet_friendly\": true,");
                                    }
                                    else
                                    {
                                        results_builder.Append("  \"is_pet_friendly\": false,");
                                    }

                                    results_builder.Append("  \"feefo_average\": " + feefo_average + ",");
                                    results_builder.Append("  \"features\": [");
                                    results_builder.Append(GetFeatures(property_code));
                                    results_builder.Append("  ]");
                                    results_builder.Append("},");
                                }
                            }

                            if (classes.common.NullSafeString(json.search.mode) != "LASTMINUTE" && classes.common.NullSafeString(json.search.mode) != "LATEAVAILABILITY")
                            {
                                if (classes.common.NullSafeInteger(json.search.page) < 2 && classes.common.NullSafeString(json.search.variables).Contains("2877") == false)
                                {
                                    int region = 0;
                                    int county = 0;

                                    if (json.search.region.Contains("r_"))
                                    {
                                        region = classes.common.NullSafeInteger(json.search.region.Replace("r_", ""));
                                    }

                                    if (json.search.region.Contains("c_"))
                                    {
                                        county = classes.common.NullSafeInteger(json.search.region.Replace("c_", ""));
                                    }

                                    classes.api.SaveSearch(region, county, json.search.town, json.search.sleeps, "", json.search.arrival_date, nights, json.search.features, "API");
                                }
                            }

                            json_content.Append("{");
                            json_content.Append("   \"api\":{");
                            json_content.Append("       \"success\": true,");
                            json_content.Append("       \"authenticated\": true,");
                            json_content.Append("       \"messages\": [");
                            json_content.Append(additional_message.ToString().TrimEnd(','));
                            json_content.Append("       ]");
                            json_content.Append("   },");
                            json_content.Append("   \"results\":{");
                            json_content.Append("       \"records\": " + record_count + ",");
                            json_content.Append("       \"pages\": " + pages_needed);
                            json_content.Append("   },");
                            json_content.Append("   \"properties\": [");
                            json_content.Append(results_builder.ToString().TrimEnd(','));
                            json_content.Append("   ]");
                            json_content.Append("}");
                        }
                        catch (Exception ex)
                        {
                            System.Text.StringBuilder error_builder = new System.Text.StringBuilder();
                            error_builder.AppendLine("Please debug and fix.");
                            error_builder.AppendLine("");
                            error_builder.AppendLine("Mode: " + classes.common.NullSafeString(json.search.mode));
                            error_builder.AppendLine("Property: " + classes.common.NullSafeString(json.search.property));
                            error_builder.AppendLine("Region: " + classes.common.NullSafeString(json.search.region));
                            error_builder.AppendLine("Town: " + classes.common.NullSafeString(json.search.town));
                            error_builder.AppendLine("Arrival Date: " + classes.common.NullSafeString(json.search.arrival_date));
                            error_builder.AppendLine("Departure Date: " + classes.common.NullSafeString(json.search.departure_date));
                            error_builder.AppendLine("Sleeps: " + classes.common.NullSafeString(json.search.sleeps));
                            error_builder.AppendLine("Bedrooms: " + classes.common.NullSafeString(json.search.bedrooms));
                            error_builder.AppendLine("Min Price: " + classes.common.NullSafeString(json.search.min_price));
                            error_builder.AppendLine("Max Price: " + classes.common.NullSafeString(json.search.max_price));
                            error_builder.AppendLine("Flexibility: " + classes.common.NullSafeString(json.search.flexibility));
                            error_builder.AppendLine("Features: " + classes.common.NullSafeString(json.search.features));
                            error_builder.AppendLine("Variables: " + classes.common.NullSafeString(json.search.variables));
                            error_builder.AppendLine("Late Availability Days: " + classes.common.NullSafeString(json.search.late_availability_days));
                            error_builder.AppendLine("Late Availability Count: " + classes.common.NullSafeString(json.search.late_availability_days_count));
                            error_builder.AppendLine("Offer CutOff: " + classes.common.NullSafeString(json.search.offer_cutoff));
                            error_builder.AppendLine("Offer Category ID: " + classes.common.NullSafeLong(json.search.offer_category_id));
                            error_builder.AppendLine("Order By: " + classes.common.NullSafeString(json.search.order_by));
                            error_builder.AppendLine("Properties Per Page: " + classes.common.NullSafeString(json.search.properties_per_page));
                            error_builder.AppendLine("Page: " + classes.common.NullSafeString(json.search.page));
                            error_builder.AppendLine("");
                            error_builder.AppendLine(ex.ToString());

                            classes.common.SendEmail("Search Failure", error_builder.ToString());
                        }
                    }
                }
                else
                {
                    json_content.Append("{");
                    json_content.Append("   \"api\":{");
                    json_content.Append("       \"success\": false,");
                    json_content.Append("       \"messages\": [");
                    json_content.Append("           \"JSON request is not valid\"");
                    json_content.Append("       ]");
                    json_content.Append("   }");
                    json_content.Append("}");
                }
            }
            else
            {
                json_content.Append("{");
                json_content.Append("   \"api\":{");
                json_content.Append("       \"success\": false,");
                json_content.Append("       \"messages\": [");
                json_content.Append("           \"JSON request is empty\"");
                json_content.Append("       ]");
                json_content.Append("   }");
                json_content.Append("}");
            }

            Response.Write(json_content.ToString());
        }

        private static string GetFeatures(long property_code)
        {
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT category_id, variable_name, variable_order, tblSC_PropertyVariables.variable_id");
            sql_query.AppendLine("FROM tblSC_PropertyVariables");
            sql_query.AppendLine("INNER JOIN tblSC_SearchVariables ON tblSC_PropertyVariables.variable_id = tblSC_SearchVariables.variable_id");
            sql_query.AppendLine("WHERE property_code = " + property_code + " AND category_id = 477");
            sql_query.AppendLine("ORDER BY variable_order ASC");

            System.Text.StringBuilder features = new System.Text.StringBuilder();

            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query.ToString(), connection);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string variable_name = HttpUtility.HtmlEncode(classes.common.NullSafeString(reader["variable_name"]));

                    features.Append("\"" + variable_name + "\",");
                }
            }

            return features.ToString().TrimEnd(',');
        }
    }
}