using mulberry.api.classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using static mulberry.api.classes.search_availability;
using static mulberry.api.classes.search_details;
using static mulberry.api.classes.search_rentabilities;

namespace mulberry.api._classes
{
    public class PropertyCalendar
    {
        public Details PropertyDetails { get; set; }
        public Availability PropertyAvailability { get; set; }
        public Rentabilities PropertyRentabilities { get; set; }

        // get property for new calendar
        public static string GetPropertiesCalendar(int property_resource_id)
        {
            PropertyCalendar propertyCalendar = new PropertyCalendar();
            string json_response = "";
            propertyCalendar.PropertyDetails = search_details.SearchResourceIdDetails(property_resource_id);
            propertyCalendar.PropertyAvailability = search_availability.SearchResourceIdAvailability(property_resource_id);
            propertyCalendar.PropertyRentabilities = search_rentabilities.SearchResourceIdRentabilities(property_resource_id);
            json_response = JsonConvert.SerializeObject(propertyCalendar);

            return json_response;
        }


        public static string Stopper()
        {
            string json_response = "";
            PropertyCalendar propertyCalendar = new PropertyCalendar
            {
                PropertyDetails = new Details
                {
                    PropertyPets = 2,
                    PropertySleeps = 8,
                    PropertySleepsAdults = 5,
                    PropertySleepsChildren = 2,
                    PropertySleepsInfants = 1
                },
                PropertyAvailability = new Availability
                {
                    PropertyAllocatable = 1,
                    PropertyReservable = 1,
                    PropertyReserved = 0
                },
                PropertyRentabilities = new Rentabilities
                {
                    PropertyBookDateFrom = Convert.ToDateTime("2018-09-21T00:00:00+03:00"),
                    PropertyBookDateTo = Convert.ToDateTime("2018-09-27T00:00:00+03:00"),
                    PropertyDayOfWeek = "MON WED FRI SUN",
                    PropertyMaxDuration = 10,
                    PropertyMinDuration = 2,
                    PropertyType = "START"
                }
            };
            try
            {
                json_response = JsonConvert.SerializeObject(propertyCalendar, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return json_response;
        }
    }
}