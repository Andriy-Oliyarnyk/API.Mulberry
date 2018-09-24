using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mulberry.api.search.result_monitoring_page
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int site_id = 495;
            string name_table = "tblSC_PropertyDetails";
            string count_property = ReturnCountProperty(name_table, site_id);

            var json = JsonConvert.SerializeObject(count_property);
            Response.Write(json.ToString());
        }

        private string ReturnCountProperty(string nameTable, int site_id)
        {
            string count_property = "";
            string str = "SELECT COUNT(*) FROM " + nameTable + " WHERE site_id=" + site_id;
            
            try
            {
                using (SqlConnection connection = new SqlConnection(classes.common.connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(str, connection);
                    count_property = Convert.ToString(command.ExecuteScalar());

                    connection.Close();
                }

                return count_property;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}