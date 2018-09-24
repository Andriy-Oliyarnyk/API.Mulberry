using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace mulberry.api._temp
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string property = Regex.Replace("Pound Cottage & Annexe".Trim(), @"[^A-Za-z0-9\- , ' &]", "").Trim().Replace("'", "''");

            Response.Write(property);
        }
    }
}