using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace mulberry.api.classes
{
    public class search_property
    {
        public static string[] ResultsPropertyCodes(long site_id, string mode, bool is_supercontrol_down, string region, long town, string arrival_date, string departure_date, string sleeps, int bedrooms, int min_price, int max_price, int flexibility, string features, string variables, int late_availability_days, int late_availability_days_count, string offer_cutoff, long offer_category_id, string property)
        {
            string[] str = new string[2];
            string results_property_codes = "";
            string property_codes = "";
            int nights = Nights(arrival_date, departure_date);

            if ((arrival_date != "") || (mode == "LASTMINUTE" || mode == "LATEAVAILABILITY"))
            {
                if (is_supercontrol_down == false)
                {
                    if (mode != "LASTMINUTE" && mode != "LATEAVAILABILITY")
                    {
                        results_property_codes = GetPropertyCodes(site_id, mode, is_supercontrol_down, region, town, "", "", sleeps, bedrooms, min_price, max_price, flexibility, features, variables, late_availability_days, late_availability_days_count, offer_cutoff, offer_category_id, property);
                    }

                    property_codes = RunSuperControl(results_property_codes, mode, 0, "", arrival_date, nights, sleeps, bedrooms, flexibility, features, late_availability_days, late_availability_days_count, min_price, max_price);
                }
            }
            str[0] = results_property_codes != "" ? results_property_codes : "";
            str[1] = property_codes != "" ? property_codes : "";
            return str;
        }

        public static int Nights(string arrival_date, string departure_date)
        {
            if (departure_date != "")
            {
                DateTime date1 = Convert.ToDateTime(arrival_date);
                DateTime date2 = Convert.ToDateTime(departure_date);

                TimeSpan nights_calc = date2 - date1;
                int nights = common.NullSafeInteger(nights_calc.Days);

                return nights;
            }
            return 0;
        }

        public static string Run(long site_id, string mode, bool is_supercontrol_down, string region, long town, string arrival_date, string departure_date, string sleeps, int bedrooms, int min_price, int max_price, int flexibility, string features, string variables, int late_availability_days, int late_availability_days_count, string offer_cutoff, long offer_category_id, string property, int return_count = 0, string results_property_codes = "", string property_codes = "")
        {
            int nights = Nights(arrival_date, departure_date);

            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            //mode = mode;
            property = Regex.Replace(property.Trim(), @"[^A-Za-z0-9\- , ' \. &]", "").Trim().Replace("'", "''");
            //region = region;
            //town = town;
            //sleeps = sleeps;
            //bedrooms = bedrooms;
            //min_price = min_price;
            //max_price = max_price;
            //flexibility = flexibility;
            //features = features;
            //variables = variables;

            if (return_count == 1)
            {
                sql_query.AppendLine("SELECT COUNT(DISTINCT tblSC_PropertyDetails.property_code)");
            }
            else
            {
                string offer_sql = "";
                if (arrival_date != "" && departure_date != "")
                {
                    offer_sql = "(SELECT TOP 1 tblCMS_PropertyOffers.offer_text FROM tblCMS_PropertyOffers WHERE tblCMS_PropertyOffers.property_code = tblSC_PropertyDetails.property_code AND '" + classes.common.NullSafeDate(arrival_date).ToString("dd/MM/yyyy 00:00:00") + "' >= offer_start_date AND '" + classes.common.NullSafeDate(departure_date).ToString("dd/MM/yyyy 23:59:59") + "' <= offer_end_date)";
                }
                else
                {
                    offer_sql = "(SELECT TOP 1 tblCMS_PropertyOffers.offer_text FROM tblCMS_PropertyOffers WHERE tblCMS_PropertyOffers.property_code = tblSC_PropertyDetails.property_code AND '" + DateTime.Now.ToString("dd/MM/yyyy 23:59:59") + "' <= offer_end_date ORDER BY offer_start_date ASC)";
                }

                sql_query.AppendLine("SELECT DISTINCT tblSC_PropertyDetails.property_code, property_order, property_name, property_town, property_postcode, property_sleeps, property_sleeps_text, property_bedrooms, property_bathrooms, CONVERT(varchar(1000), property_search_description) AS property_search_description, property_minprice, property_availability_link, property_lat, property_long, property_1_night, property_2_night, property_3_night, property_4_night, property_5_night, property_6_night, property_deal_text, property_deal_date_start, property_deal_date_end, property_price_order, region_name, property_price, property_booking, thumb_image = (SELECT TOP 1 property_image_thumb FROM tblSC_PropertyImages WHERE tblSC_PropertyImages.property_code = tblSC_PropertyDetails.property_code AND property_image_default = 1), is_new = (SELECT COUNT(property_code) FROM tblSC_PropertyVariables WHERE variable_id = 7735 AND tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code), featured = (SELECT TOP 1 variable_id FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code AND variable_id = 2877), feefo_average = (SELECT ROUND((SUM(review_rating) / COUNT(review_id)), 0) FROM tblCottageReviews WHERE cottage_id = tblSC_PropertyDetails.property_code), pet_friendly = (SELECT COUNT(variable_id) FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code AND tblSC_PropertyVariables.variable_id = 2313), offer_text = " + offer_sql);
            }

            sql_query.AppendLine("FROM tblSC_PropertyDetails");
            sql_query.AppendLine("INNER JOIN tblSC_Regions ON tblSC_PropertyDetails.property_region_0 = tblSC_Regions.region_id");
            sql_query.AppendLine("LEFT JOIN tblExtraPropertyInformation ON tblSC_PropertyDetails.property_code = tblExtraPropertyInformation.property_code");

            if (results_property_codes == "")
            {
                sql_query.AppendLine("LEFT JOIN tblSearchRegionsCounties ON tblSC_PropertyDetails.property_region_0 = tblSearchRegionsCounties.county_id");
                sql_query.AppendLine("LEFT JOIN tblSearchPropertyExtraTowns ON tblSC_PropertyDetails.property_code = tblSearchPropertyExtraTowns.property_code");
                sql_query.AppendLine("LEFT JOIN tblSearchPropertyExtraCounties ON tblSC_PropertyDetails.property_code = tblSearchPropertyExtraCounties.property_code");
                sql_query.AppendLine("LEFT JOIN tblSearchCountyExtraTowns ON tblSearchPropertyExtraTowns.town_id = tblSearchCountyExtraTowns.town_id");
            }

            sql_query.AppendLine("WHERE tblSC_PropertyDetails.property_code > 0 AND tblSC_PropertyDetails.site_id = " + site_id + " AND property_is_open_golf = 0");

            if (results_property_codes == "")
            {
                if (region.Contains("r_"))
                {
                    sql_query.AppendLine(" AND tblSearchRegionsCounties.region_id = " + classes.common.NullSafeInteger(region.Replace("r_", "")));
                }

                if (region.Contains("c_"))
                {
                    sql_query.AppendLine(" AND (tblSC_PropertyDetails.property_region_0 = " + classes.common.NullSafeInteger(region.Replace("c_", "")) + " OR tblSearchPropertyExtraCounties.county_id = " + classes.common.NullSafeInteger(region.Replace("c_", "")) + " OR tblSearchCountyExtraTowns.county_id = " + classes.common.NullSafeInteger(region.Replace("c_", "")) + ")");
                }

                if (town != 0)
                {
                    sql_query.AppendLine(" AND tblSearchPropertyExtraTowns.town_id = " + town);
                }

                if (sleeps != "")
                {
                    switch (sleeps)
                    {
                        case "1-4":
                            variables = "105202," + variables; // small
                            break;

                        case "5-9":
                            variables = "105203," + variables; // medium
                            break;

                        case "10-15":
                            variables = "105204," + variables; // large
                            break;

                        case "16":
                            variables = "105205," + variables; // grand
                            break;

                        case "1":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + sleeps + " AND " + 2);
                            break;

                        case "2":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 2 + " AND " + 4);
                            break;

                        case "3":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 3 + " AND " + 5);
                            break;

                        case "4":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 4 + " AND " + 6);
                            break;

                        case "5":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 5 + " AND " + 8);
                            break;

                        case "6":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 6 + " AND " + 9);
                            break;

                        case "7":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 7 + " AND " + 10);
                            break;

                        case "8":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 8 + " AND " + 11);
                            break;

                        case "9":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 9 + " AND " + 12);
                            break;

                        case "10":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 10 + " AND " + 13);
                            break;

                        case "12":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 12 + " AND " + 16);
                            break;

                        case "14":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 14 + " AND " + 18);
                            break;

                        case "15":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 15 + " AND " + 18);
                            break;

                        case "18":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 18 + " AND " + 22);
                            break;

                        case "20":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 20 + " AND " + 24);
                            break;

                        case "24":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 24 + " AND " + 28);
                            break;

                        case "28":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 28 + " AND " + 32);
                            break;

                        case "30":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 30 + " AND " + 99);
                            break;

                        case "40":
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 40 + " AND " + 99);
                            break;

                        default:
                            // do nothing
                            break;
                    }
                }

                if (mode == "LHC")
                {
                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 10 + " AND " + 99);
                }

                if (bedrooms != 0)
                {
                    if (bedrooms == 10)
                    {
                        sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms BETWEEN " + bedrooms + " AND " + 100);
                    }
                    else
                    {
                        if (bedrooms == 1 || bedrooms == 2)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms = " + bedrooms);
                        }
                        else
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms BETWEEN " + bedrooms + " AND " + (bedrooms + 1));
                        }
                    }
                }

                if (nights == 0)
                {
                    if (min_price != 0 || max_price != 0)
                    {
                        if (min_price != 0 && max_price == 0)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_minprice >= " + min_price);
                        }

                        if (min_price == 0 && max_price != 0)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_minprice <= " + max_price);
                        }

                        if (min_price != 0 && max_price != 0)
                        {
                            if (min_price <= max_price)
                            {
                                sql_query.AppendLine(" AND tblSC_PropertyDetails.property_price_order BETWEEN " + min_price + " AND " + max_price);
                            }
                        }
                    }
                }

                if (features != "")
                {
                    if (features.Contains(","))
                    {
                        string[] split_property_feature = features.Split(',');

                        sql_query.Append(" AND (");

                        for (int i = 0; i < split_property_feature.Length; i++)
                        {
                            if (i != 0)
                            {
                                sql_query.Append(" AND ");
                            }

                            sql_query.Append("(',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + split_property_feature[i] + ",%'");
                        }

                        sql_query.Append(")");
                    }
                    else
                    {
                        sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + features + ",%'");
                    }
                }

                if (variables != "")
                {
                    variables = variables.TrimEnd(',');

                    if (variables.Contains(","))
                    {
                        string[] split_property_variable = variables.Split(',');

                        sql_query.Append(" AND (");

                        for (int i = 0; i < split_property_variable.Length; i++)
                        {
                            if (i != 0)
                            {
                                sql_query.Append(" AND ");
                            }

                            sql_query.Append("(',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + split_property_variable[i] + ",%'");
                        }

                        sql_query.Append(")");
                    }
                    else
                    {
                        sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + variables + ",%'");
                    }
                }
            }

            if (property_codes != "")
            {
                if (property_codes.Contains(","))
                {
                    string[] split_property_codes = property_codes.Split(',');

                    sql_query.Append(" AND (");

                    for (int i = 0; i < split_property_codes.Length; i++)
                    {
                        if (i != 0)
                        {
                            sql_query.Append(" OR ");
                        }

                        sql_query.Append("tblSC_PropertyDetails.property_code = " + split_property_codes[i]);
                    }

                    sql_query.Append(")");
                }
                else
                {
                    sql_query.Append(" AND tblSC_PropertyDetails.property_code = " + property_codes);
                }
            }

            if (offer_cutoff != "")
            {
                sql_query.Append(" AND property_deal_text <> '' AND '" + DateTime.Now.ToString("dd/MM/yyyy") + " 00:00:00' > property_deal_date_start AND '" + DateTime.Now.ToString("dd/MM/yyyy") + " 23:59:59' < property_deal_date_end");
            }

            if (results_property_codes == "")
            {
                if (property != "")
                {
                    sql_query.Append(" AND (tblSC_PropertyDetails.property_code = " + classes.common.NullSafeLong(property) + " OR tblSC_PropertyDetails.property_name LIKE '" + property + "%')");
                }

                if (nights > 0)
                {
                    if (min_price != 0 && max_price != 0)
                    {
                        if (nights == 1)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_1_night <> 0");
                        }

                        if (nights == 2)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_2_night <> 0");
                        }

                        if (nights == 3)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_3_night <> 0");
                        }

                        if (nights == 4)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_4_night <> 0");
                        }

                        if (nights == 5)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_5_night <> 0");
                        }

                        if (nights >= 6)
                        {
                            sql_query.AppendLine(" AND tblSC_PropertyDetails.property_6_night <> 0");
                        }
                    }
                }
            }

            if (results_property_codes == "")
            {
                // Offer Category Handling
                if (offer_category_id != 0)
                {
                    classes.cms_offer_category_info cms_offer_category_info = classes.cms_offer_category.GetInfo(offer_category_id);

                    sql_query.Append(" AND property_deal_text <> '' AND '" + DateTime.Now.ToString("dd/MM/yyyy") + " 00:00:00' > property_deal_date_start");

                    if (cms_offer_category_info.selection == 1)
                    {
                        // Property
                        string[] split_property_codes = cms_offer_category_info.properties;

                        sql_query.Append(" AND (");

                        for (int i = 0; i < split_property_codes.Length; i++)
                        {
                            if (i != 0)
                            {
                                sql_query.Append(" OR ");
                            }

                            sql_query.Append("tblSC_PropertyDetails.property_code = " + split_property_codes[i]);
                        }

                        sql_query.Append(")");
                    }

                    if (cms_offer_category_info.selection == 2)
                    {
                        // Search Based
                        if (cms_offer_category_info.search_region_county.Contains("r_"))
                        {
                            sql_query.AppendLine(" AND tblSearchRegionsCounties.region_id = " + classes.common.NullSafeInteger(cms_offer_category_info.search_region_county.Replace("r_", "")));
                        }

                        if (cms_offer_category_info.search_region_county.Contains("c_"))
                        {
                            sql_query.AppendLine(" AND (tblSC_PropertyDetails.property_region_0 = " + classes.common.NullSafeInteger(cms_offer_category_info.search_region_county.Replace("c_", "")) + ")");
                        }

                        if (cms_offer_category_info.search_town != 0)
                        {
                            sql_query.AppendLine(" AND tblSearchPropertyExtraTowns.town_id = " + cms_offer_category_info.search_town);
                        }

                        if (cms_offer_category_info.search_sleeps != "")
                        {
                            switch (cms_offer_category_info.search_sleeps)
                            {
                                case "1-4":
                                    // small
                                    sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%,105202,%'");
                                    break;

                                case "5-9":
                                    // medium
                                    sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%,105203,%'");
                                    break;

                                case "10-15":
                                    // large
                                    sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%,105204,%'");
                                    break;

                                case "16":
                                    // grand
                                    sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%,105205,%'");
                                    break;

                                case "1":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN 1 AND " + 2);
                                    break;

                                case "2":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 2 + " AND " + 3);
                                    break;

                                case "3":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 3 + " AND " + 5);
                                    break;

                                case "4":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 4 + " AND " + 6);
                                    break;

                                case "5":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 5 + " AND " + 8);
                                    break;

                                case "6":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 6 + " AND " + 9);
                                    break;

                                case "7":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 7 + " AND " + 10);
                                    break;

                                case "8":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 8 + " AND " + 11);
                                    break;

                                case "9":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 9 + " AND " + 12);
                                    break;

                                case "10":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 10 + " AND " + 13);
                                    break;

                                case "12":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 12 + " AND " + 16);
                                    break;

                                case "14":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 14 + " AND " + 18);
                                    break;

                                case "15":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 15 + " AND " + 18);
                                    break;

                                case "18":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 18 + " AND " + 22);
                                    break;

                                case "20":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 20 + " AND " + 24);
                                    break;

                                case "24":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 24 + " AND " + 28);
                                    break;

                                case "28":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 28 + " AND " + 32);
                                    break;

                                case "30":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 30 + " AND " + 99);
                                    break;

                                case "40":
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_sleeps BETWEEN " + 40 + " AND " + 99);
                                    break;

                                default:
                                    // do nothing
                                    break;
                            }
                        }

                        if (cms_offer_category_info.search_bedrooms != 0)
                        {
                            if (cms_offer_category_info.search_bedrooms == 10)
                            {
                                sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms BETWEEN " + cms_offer_category_info.search_bedrooms + " AND " + 100);
                            }
                            else
                            {
                                if (cms_offer_category_info.search_bedrooms == 1 || cms_offer_category_info.search_bedrooms == 2)
                                {
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms = " + cms_offer_category_info.search_bedrooms);
                                }
                                else
                                {
                                    sql_query.AppendLine(" AND tblSC_PropertyDetails.property_bedrooms BETWEEN " + cms_offer_category_info.search_bedrooms + " AND " + (cms_offer_category_info.search_bedrooms + 1));
                                }
                            }
                        }

                        long[] split_property_feature = cms_offer_category_info.features;

                        if (split_property_feature.Length > 0 && split_property_feature[0] != 0)
                        {
                            sql_query.Append(" AND (");

                            for (int i = 0; i < split_property_feature.Length; i++)
                            {
                                if (i != 0)
                                {
                                    sql_query.Append(" AND ");
                                }

                                sql_query.Append("(',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + split_property_feature[i] + ",%'");
                            }

                            sql_query.Append(")");
                        }

                        if (cms_offer_category_info.search_variable != "")
                        {
                            cms_offer_category_info.search_variable = cms_offer_category_info.search_variable.TrimEnd(',');

                            if (cms_offer_category_info.search_variable.Contains(","))
                            {
                                string[] split_property_variable = cms_offer_category_info.search_variable.Split(',');

                                sql_query.Append(" AND (");

                                for (int i = 0; i < split_property_variable.Length; i++)
                                {
                                    if (i != 0)
                                    {
                                        sql_query.Append(" AND ");
                                    }

                                    sql_query.Append("(',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + split_property_variable[i] + ",%'");
                                }

                                sql_query.Append(")");
                            }
                            else
                            {
                                sql_query.Append(" AND (',' + STUFF((SELECT ',', variable_id AS [text()] FROM tblSC_PropertyVariables WHERE tblSC_PropertyVariables.property_code = tblSC_PropertyDetails.property_code FOR XML PATH ('')), 1, 1, '') + ',') LIKE '%," + cms_offer_category_info.search_variable + ",%'");
                            }
                        }

                        if (classes.common.NullSafeString(cms_offer_category_info.date_start) != "" && classes.common.NullSafeString(cms_offer_category_info.date_end) != "")
                        {
                            sql_query.Append(" AND '" + classes.common.NullSafeDate(cms_offer_category_info.date_start).ToString("dd/MM/yyyy") + " 00:00:00' > property_deal_date_start AND property_deal_date_end < '" + classes.common.NullSafeDate(cms_offer_category_info.date_end).ToString("dd/MM/yyyy") + " 23:59:59'");
                        }
                    }
                }
            }

            if (return_count == 0)
            {
                sql_query.AppendLine(" ORDER BY property_order DESC");
            }

            //HttpContext.Current.Response.Write(sql_query.ToString());

            //if(offer_category_id == 39)
            //{
            //    classes.common.SendEmail("debug", sql_query.ToString());
            //}

            return sql_query.ToString();
        }

        public static string RunSuperControl(string property_code_list, string mode, long county, string region, string arrival_date, int nights, string sleeps, int bedrooms, int flexibility, string features, int late_availability_days, int late_availability_days_count, int min_price, int max_price)
        {
            System.Text.StringBuilder property_codes = new System.Text.StringBuilder();

            string xml_county = (county != 0 ? "&region=" + county : "");
            string xml_location = "";

            if (region.Contains(","))
            {
                xml_location = "&region_1_list=" + region;
            }
            else
            {
                xml_location = (region != "0" ? "&region_1=" + region : "");
            }
            
            string xml_start_date = (arrival_date != "" ? "&startdate=" + classes.common.NullSafeDate(arrival_date).ToString("yyyy-MM-dd") : "");
            string xml_nights = (nights != 0 ? "&numbernights=" + nights : "");

            string xml_sleeps = "";
            if (sleeps != "")
            {
                switch (sleeps)
                {
                    case "1-4":
                        features = "105202," + features; // small
                        break;

                    case "5-9":
                        features = "105203," + features; // medium
                        break;

                    case "10-15":
                        features = "105204," + features; // large
                        break;

                    case "16":
                        features = "105205," + features; // grand
                        break;

                    case "1":
                        xml_sleeps = "&sleeps=" + sleeps + "&sleeps_range=1";
                        break;

                    case "2":
                        xml_sleeps = "&sleeps=" + sleeps + "&sleeps_range=3";
                        break;

                    case "3":
                        xml_sleeps = "&sleeps=" + 3 + "&sleeps_range=" + 2;
                        break;

                    case "4":
                        xml_sleeps = "&sleeps=" + 4 + "&sleeps_range=" + 2;
                        break;

                    case "5":
                        xml_sleeps = "&sleeps=" + 5 + "&sleeps_range=" + 3;
                        break;

                    case "6":
                        xml_sleeps = "&sleeps=" + 6 + "&sleeps_range=" + 3;
                        break;

                    case "7":
                        xml_sleeps = "&sleeps=" + 7 + "&sleeps_range=" + 3;
                        break;

                    case "8":
                        xml_sleeps = "&sleeps=" + 8 + "&sleeps_range=" + 3;
                        break;

                    case "9":
                        xml_sleeps = "&sleeps=" + 9 + "&sleeps_range=" + 3;
                        break;

                    case "10":
                        xml_sleeps = "&sleeps=" + 10 + "&sleeps_range=" + 3;
                        break;

                    case "12":
                        xml_sleeps = "&sleeps=" + 12 + "&sleeps_range=" + 4;
                        break;

                    case "14":
                        xml_sleeps = "&sleeps=" + 14 + "&sleeps_range=" + 4;
                        break;

                    case "18":
                        xml_sleeps = "&sleeps=" + 18 + "&sleeps_range=" + 4;
                        break;

                    case "20":
                        xml_sleeps = "&sleeps=" + 20 + "&sleeps_range=" + 4;
                        break;

                    case "24":
                        xml_sleeps = "&sleeps=" + 24 + "&sleeps_range=" + 8;
                        break;

                    case "28":
                        xml_sleeps = "&sleeps=" + 28 + "&sleeps_range=" + 8;
                        break;

                    case "30":
                        xml_sleeps = "&sleeps=" + 30 + "&sleeps_range=" + 99;
                        break;

                    case "40":
                        xml_sleeps = "&sleeps=" + 40 + "&sleeps_range=" + 99;
                        break;

                    default:
                        // do nothing
                        break;
                }
            }

            if (mode == "LHC")
            {
                xml_sleeps = "&sleeps=" + 10 + "&sleeps_range=" + 99;
            }

            string xml_bedrooms = (bedrooms != 0 ? "&bedrooms=" + bedrooms : "");
            string xml_flexibility = (flexibility != 0 ? "&datevar=" + flexibility : "");

            string xml_features = "";
            System.Text.StringBuilder features_sc = new System.Text.StringBuilder();
            if (features != "")
            {
                features = features.TrimEnd(','); 

                if (features.Contains(",") == true)
                {
                    //Split variables for searching advanced
                    string[] features_split = features.Split(',');
                    foreach (string split_feature in features_split)
                    {
                        features_sc.Append(split_feature + "-1,");
                    }

                    features = features_sc.ToString().TrimEnd(',');
                }
                else
                {
                    features = features + "-1,";
                }
            }

            if (features != "")
            {
                xml_features = "&variableID=" + features + "&searchtype=AND";
            }

            string xml_property_list = "";
            if (property_code_list != "")
            {
                xml_property_list = "&propertylist=" + property_code_list;
            }


            //Generate SuperControl URL
            string url = "http://api.supercontrol.co.uk/xml/filter3.asp?siteID=495" + xml_county + xml_location + xml_bedrooms + xml_sleeps + xml_start_date + xml_nights + xml_flexibility + xml_features + xml_property_list + "&basic_details=1&no_price_err=1";

            if (mode == "LASTMINUTE" || mode == "LATEAVAILABILITY")
            {
                url = "http://api.supercontrol.co.uk/xml/filter3.asp?siteID=495&lateavail_days=" + late_availability_days + "&lateavail_days_count=" + late_availability_days_count + "&lateavail_days_date=" + classes.common.NullSafeDate(arrival_date).ToString("yyyy-MM-dd") + "&basic_details=1";
            }

            //classes.common.SendEmail("Debug", url);

            //HttpContext.Current.Response.Write(url);
            //HttpContext.Current.Response.End();

            try
            {
                XDocument doc = XDocument.Load(url);
                var data = from item in doc.Descendants("property")
                           select new
                           {
                               PropertyCode = item.Element("propertycode").Value.ToString(),
                               PropertyPrices = item.Elements("prices")
                           };

                foreach (var item in data)
                {
                    if (min_price == 0 && max_price == 0)
                    {
                        property_codes.Append(item.PropertyCode + ",");
                    }
                    else
                    {
                        foreach (var price_band in item.PropertyPrices)
                        {
                            foreach (var param in price_band.Elements("price"))
                            {
                                decimal supercontrol_price = classes.common.NullSafeDecimal(param.Element("rate").Value, 0.00M);
                                decimal supercontrol_discount = classes.common.NullSafeDecimal(param.Element("discount").Value, 0.00M);
                                decimal dicounted_price = supercontrol_price - supercontrol_discount;


                                if (min_price != 0 && max_price == 0)
                                {
                                    if (dicounted_price >= min_price)
                                    {
                                        property_codes.Append(item.PropertyCode + ",");
                                    }
                                }

                                if (min_price == 0 && max_price != 0)
                                {
                                    if (dicounted_price <= max_price)
                                    {
                                        property_codes.Append(item.PropertyCode + ",");
                                    }
                                }

                                if (min_price != 0 && max_price != 0)
                                {
                                    if (dicounted_price >= min_price && dicounted_price <= max_price)
                                    {
                                        property_codes.Append(item.PropertyCode + ",");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               supercontrol.StoreError(1, ex.ToString(), "API URL: " + url);
            }

            if (property_codes.ToString().Length == 0)
            {
                return "0";
            }

            return property_codes.ToString().TrimEnd(',');
        }

        public static long Count(string sql_query)
        {
            long count = 0;

            using (SqlConnection connection = new SqlConnection(common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query.ToString(), connection);
                command.CommandType = System.Data.CommandType.Text;

                count = classes.common.NullSafeInteger(command.ExecuteScalar());
            }

            return count;
        }

        private static string GetCountyAdditionalTowns(long county_id)
        {
            System.Text.StringBuilder sql_query = new System.Text.StringBuilder();
            sql_query.AppendLine("SELECT region_id");
            sql_query.AppendLine("FROM tblSC_Regions");
            sql_query.AppendLine("WHERE region_parent_id = " + county_id);
            sql_query.AppendLine("UNION");
            sql_query.AppendLine("SELECT town_id");
            sql_query.AppendLine("FROM tblSearchCountyExtraTowns");
            sql_query.AppendLine("WHERE county_id = " + county_id);


            System.Text.StringBuilder content = new System.Text.StringBuilder();

            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query.ToString(), connection);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long region_id = classes.common.NullSafeLong(reader["region_id"]);

                    content.Append(region_id + ",");
                }
            }

            return content.ToString().TrimEnd(',');
        }

        private static string GetPropertyCodes(long site_id, string mode, bool is_supercontrol_down, string region, long town, string arrival_date, string departure_date, string sleeps, int bedrooms, int min_price, int max_price, int flexibility, string features, string variables, int late_availability_days, int late_availability_days_count, string offer_cutoff, long offer_category_id, string property)
        {
            string sql_query = Run(site_id, mode, is_supercontrol_down, region, town, "", "", sleeps, bedrooms, min_price, max_price, flexibility, features, variables, late_availability_days, late_availability_days_count, offer_cutoff, offer_category_id, property);

            System.Text.StringBuilder content = new System.Text.StringBuilder();

            using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql_query, connection);
                command.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    long property_code = classes.common.NullSafeLong(reader["property_code"]);

                    content.Append(property_code + ",");
                }
            }

            return content.ToString().TrimEnd(',');
        }
    }
}