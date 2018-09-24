using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;

namespace mulberry.api.classes
{
    public class common
    {
        public static string NullSafeString(object arg, string returnIfEmpty = "")
        {
            string returnValue = null;

            if (object.ReferenceEquals(arg, DBNull.Value) || arg == null || object.ReferenceEquals(arg, string.Empty))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToString(arg).Trim();
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        public static int NullSafeInteger(object arg, int returnIfEmpty = 0)
        {
            int returnValue = 0;

            if (object.ReferenceEquals(arg, DBNull.Value) || arg == null || object.ReferenceEquals(arg, string.Empty))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToInt32(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        public static long NullSafeLong(object arg, long returnIfEmpty = 0)
        {
            long returnValue = 0;

            if (object.ReferenceEquals(arg, DBNull.Value) || arg == null || object.ReferenceEquals(arg, string.Empty))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToInt64(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        public static decimal NullSafeDecimal(object arg, decimal returnIfEmpty)
        {
            decimal returnValue = default(decimal);

            if (object.ReferenceEquals(arg, DBNull.Value) || arg == null || object.ReferenceEquals(arg, string.Empty))
            {
                returnValue = 0.00M;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToDecimal(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        public static System.DateTime NullSafeDate(object arg, System.DateTime returnIfEmpty = default(DateTime))
        {
            System.DateTime returnValue = default(System.DateTime);

            if ((object.ReferenceEquals(arg, DBNull.Value)) || (arg == null) || (object.ReferenceEquals(arg, string.Empty)))
            {
                returnValue = returnIfEmpty;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToDateTime(arg);
                }
                catch
                {
                    returnValue = returnIfEmpty;
                }
            }

            return returnValue;
        }

        public static bool NullSafeBoolean(object arg)
        {
            bool returnValue = false;

            if (object.ReferenceEquals(arg, DBNull.Value) || arg == null || object.ReferenceEquals(arg, string.Empty))
            {
                returnValue = false;
            }
            else
            {
                try
                {
                    returnValue = Convert.ToBoolean(arg);
                }
                catch
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        public static void SendEmail(string subject, string mail_body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("api@mulberrycottages.com", "Mulberry Cottages API");
            mail.To.Add("lee.mills@think.studio");
            mail.Subject = subject;

            mail.Body = mail_body;

            SmtpClient smtp = new SmtpClient("smtp.thinkdedicated.com");
            smtp.Send(mail);
        }

        public static string connectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DatabaseConn"].ConnectionString; }
        }

        public static string connectionStringDEV
        {
            get { return "server=backend.mulberrycottages.com;database=mulberrycottages.com_DEV;UID=devmulberry;PWD=T2ghf82hg$;Integrated Security=False;"; }
        }

        // connection to the database
        public static string NewConnectionString
        {
            get { return "server=maxx.mulberrycottages.com;database=mulberrycottages.com;UID=maxxdev;PWD=wuDF233hgf23or£$;Integrated Security=False;"; }
        }
    }
}