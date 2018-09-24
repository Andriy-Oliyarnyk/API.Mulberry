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
using System.Xml.Linq;

namespace mulberry.api.property.get_price
{
    public class Api
    {
        public string key { get; set; }
        public bool test_mode { get; set; }
    }

    public class Property
    {
        public int property_code { get; set; }
        public string arrival_date { get; set; }
        public string departure_date { get; set; }
        public int is_details { get; set; }
    }

    public class RootObject
    {
        public Api api { get; set; }
        public Property property { get; set; }
    }

    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Text.StringBuilder json_content = new System.Text.StringBuilder();
            string postData = new System.IO.StreamReader(Request.InputStream).ReadToEnd();

            //json_content.AppendLine("{");
            //json_content.AppendLine("   \"api\":{");
            //json_content.AppendLine("       \"key\": \"78g878agda78sdgad98abd\"");
            //json_content.AppendLine("   },");
            //json_content.AppendLine("   \"property\":{");
            //json_content.AppendLine("       \"property_code\": 358712,");
            //json_content.AppendLine("       \"arrival_date\": \"20/09/2016\",");
            //json_content.AppendLine("       \"departure_date\": \"30/09/2016\"");
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

                    //if (json.property.arrival_date != "")
                    //{
                    //    if (!Regex.IsMatch(json.property.arrival_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)"))
                    //    {
                    //        errors[1] = "arrival_date must be in this format DD/MM/YYYY";
                    //        err = 1;
                    //    }
                    //    else
                    //    {
                    //        if (classes.common.NullSafeDate(json.property.arrival_date) < classes.common.NullSafeDate(DateTime.Now.ToString("dd/MM/yyyy")))
                    //        {
                    //            errors[1] = "arrival_date cannot be in the past";
                    //            err = 1;
                    //        }
                    //    }
                    //}

                    //if (json.property.departure_date != "")
                    //{
                    //    if (!Regex.IsMatch(json.property.departure_date, "(?n:^(?=\\d)((?<day>31(?!(.0?[2469]|11))|30(?!.0?2)|29(?(.0?2)(?=.{3,4}(1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|0?[1-9]|1\\d|2[0-8])(?<sep>[/.-])(?<month>0?[1-9]|1[012])\\2(?<year>(1[6-9]|[2-9]\\d)\\d{2})(?:(?=\\x20\\d)\\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\\d){0,2}(?i:\\ [AP]M))|([01]\\d|2[0-3])(:[0-5]\\d){1,2})?$)"))
                    //    {
                    //        errors[2] = "departure_date must be in this format DD/MM/YYYY";
                    //        err = 1;
                    //    }
                    //    else
                    //    {
                    //        if (classes.common.NullSafeDate(json.property.departure_date) < classes.common.NullSafeDate(json.property.arrival_date))
                    //        {
                    //            errors[2] = "departure_date cannot be less than arrival_date";
                    //            err = 1;
                    //        }
                    //    }
                    //}

                    //if (json.property.arrival_date != "" || json.property.departure_date != "")
                    //{
                    //    if (json.property.arrival_date == "")
                    //    {
                    //        errors[3] = "arrival_date cannot be empty if departure_date has data";
                    //        err = 1;
                    //    }

                    //    if (json.property.departure_date == "")
                    //    {
                    //        errors[4] = "departure_date cannot be empty if arrival_date has data";
                    //        err = 1;
                    //    }
                    //}


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
                        System.Text.StringBuilder additional_message = new System.Text.StringBuilder();
                        bool is_supercontrol_down = classes.supercontrol.IsDown();
                        if (is_supercontrol_down == true)
                        {
                            additional_message.Append("\"Availability searches are currently offline, please try again later\",");
                            json.property.arrival_date = "";
                            json.property.departure_date = "";
                        }

                        string price = "";
                        int nights = 0;
                        if (json.property.arrival_date != "" && json.property.departure_date != "")
                        {
                            DateTime date1 = Convert.ToDateTime(json.property.arrival_date);
                            DateTime date2 = Convert.ToDateTime(json.property.departure_date);

                            TimeSpan nights_calc = date2 - date1;
                            nights = classes.common.NullSafeInteger(nights_calc.Days);

                            if (is_supercontrol_down == false)
                            {
                                price = RunSuperControl(json.property.property_code, json.property.arrival_date, nights, json.property.is_details);
                            }
                        }

                        if (price == "")
                        {
                            classes.cottage_info cottage_info = classes.cottage.GetInfo(json.property.property_code, api_key_info.site_id);

                            // No longer used
                            /*if (cottage_info.price_override == "")
                            {
                                if (cottage_info.min_price != 0.0M)
                                {
                                    if (json.property.is_details == 0)
                                    {
                                        price = "From <strong>" + string.Format("{0:c}", cottage_info.min_price).Replace(".00", "") + "</strong>";
                                    }
                                    else
                                    {
                                        price = string.Format("{0:c}", cottage_info.min_price).Replace(".00", "");
                                    }
                                }
                            }
                            else
                            {
                                if (cottage_info.price_override == "noprice")
                                {
                                    price = "Please enquire";
                                }
                                else
                                {
                                    if (json.property.is_details == 0)
                                    {
                                        price = "From <strong>" + cottage_info.price_override + "</strong>";
                                    }
                                    else
                                    {
                                        price = cottage_info.price_override;
                                    }
                                }
                            }*/
                            
                            if (cottage_info.property_2_night != 0 || cottage_info.property_3_night != 0 || cottage_info.property_4_night != 0)
                            {
                                if (cottage_info.property_2_night != 0)
                                {
                                    price = "Short breaks from <strong>" + string.Format("{0:c}", cottage_info.property_2_night).Replace(".00", "") + "</strong>";
                                }
                                else if (cottage_info.property_3_night != 0)
                                {
                                    price = "Short breaks from <strong>" + string.Format("{0:c}", cottage_info.property_3_night).Replace(".00", "") + "</strong>";
                                }
                                else
                                {
                                    price = "Short breaks from <strong>" + string.Format("{0:c}", cottage_info.property_4_night).Replace(".00", "") + "</strong>";
                                }
                            }
                            else
                            {
                                if (cottage_info.min_price == 0)
                                {
                                    price = "<strong>Pricing on Request</strong>";
                                }
                                else
                                {
                                    price = "Weeks from <strong>" + string.Format("{0:c}", cottage_info.min_price).Replace(".00", "") + "</strong>";
                                }
                            }
                        }

                        price = HttpUtility.HtmlEncode(price);

                        json_content.Append("{");
                        json_content.Append("   \"api\":{");
                        json_content.Append("       \"success\": true,");
                        json_content.Append("       \"authenticated\": true,");
                        json_content.Append("       \"messages\": [");
                        json_content.Append(additional_message.ToString().TrimEnd(','));
                        json_content.Append("       ]");
                        json_content.Append("   },");
                        json_content.Append("   \"property\": {");
                        json_content.Append("       \"price\": \"" + price + "\"");
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

        public static string RunSuperControl(long property_code, string arrival_date, int nights, int is_details)
        {
            string price = "";
            decimal supercontrol_price = 0.00M;
            decimal supercontrol_discount = 0.00M;
            string xml_start_date = (arrival_date != "" ? classes.common.NullSafeDate(arrival_date).ToString("yyyy-MM-dd") : "");

            try
            {
                string url = "http://api.supercontrol.co.uk/xml/filter3.asp?siteID=495&basic_details=1&startdate=" + xml_start_date + "&numbernights=" + nights + "&prices_only=1&hide_var=1&propertycode=" + property_code;

                //HttpContext.Current.Response.Write(url);
                //HttpContext.Current.Response.End();

                XDocument doc = XDocument.Load(url);
                var data = from item in doc.Descendants("property")
                           select new
                           {
                               PropertyPrices = item.Elements("prices")
                           };

                foreach (var item in data)
                {
                    foreach (var price_band in item.PropertyPrices)
                    {
                        foreach (var param in price_band.Elements("price"))
                        {
                            supercontrol_price = classes.common.NullSafeDecimal(param.Element("rate").Value, 0.00M);
                            supercontrol_discount = classes.common.NullSafeDecimal(param.Element("discount").Value, 0.00M);

                            if (supercontrol_discount != 0.00M)
                            {
                                decimal dicounted_price = supercontrol_price - supercontrol_discount;

                                if (dicounted_price != supercontrol_price)
                                {
                                    if (is_details == 0)
                                    {
                                        price = "For " + nights + " nights <s>" + string.Format("{0:c}", supercontrol_price).Replace(".00", "") + "</s> <strong>" + string.Format("{0:c}", dicounted_price).Replace(".00", "") + "</strong>";
                                    }
                                    else
                                    {
                                        price = string.Format("{0:c}", dicounted_price).Replace(".00", "");
                                    }
                                }
                                else
                                {
                                    if (is_details == 0)
                                    {
                                        price = "For " + nights + " nights <strong>" + string.Format("{0:c}", supercontrol_price).Replace(".00", "") + "</strong>";
                                    }
                                    else
                                    {
                                        price = string.Format("{0:c}", supercontrol_price).Replace(".00", "");
                                    }
                                }
                            }
                            else
                            {
                                if (is_details == 0)
                                {
                                    price = "For " + nights + " nights <strong>" + string.Format("{0:c}", supercontrol_price).Replace(".00", "") + "</strong>";
                                }
                                else
                                {
                                    price = string.Format("{0:c}", supercontrol_price).Replace(".00", "");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //SuperControl error
            }

            return price;
        }
    }
}