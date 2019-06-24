 using Microsoft.Win32;

namespace CustomerLogger.RegistryData
{
    public static class CustomerLogger_RegistryData
    {
        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public static string Admin_Password_Hashed
        {
            get 
            {
                object regValue;
                if ((regValue = Key.GetValue("Admin_Password")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set 
            {
                Key.SetValue("Admin_Password", value, RegistryValueKind.String);
            }
        }

        public static string Cougtech_Open_Time
        {
            get {
                object regValue;
                if ((regValue = Key.GetValue("Open_Time")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set {
                Key.SetValue("Open_Time", value, RegistryValueKind.String);
            }
        }

        public static string Cougtech_Close_Time
        {
            get {
                object regValue;
                if ((regValue = Key.GetValue("Close_Time")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set {
                Key.SetValue("Close_Time", value, RegistryValueKind.String);
            }
        }

        public static string Email_Ticketing_Address
        {
            get 
            {
                object regValue;
                if ((regValue = Key.GetValue("Ticket_Submission_Email_Address")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set 
            {
                Key.SetValue("Ticket_Submission_Email_Address", value, RegistryValueKind.String);
            }
        }
        
        public static string Logging_Directory
        {
            get 
            {
                object regValue;
                if ((regValue = Key.GetValue("Ticket_Logging_Directory")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set 
            {
                Key.SetValue("Ticket_Logging_Directory", value, RegistryValueKind.String);
            }
        }

        public static string Rest_Ticketing_URL
        {
            get 
            {
                object regValue;
                if ((regValue = Key.GetValue("Ticket_Submission_Rest_Url")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set 
            {
                Key.SetValue("Ticket_Submission_Rest_Url", value, RegistryValueKind.String);
            }
        }

        public static string Wsu_DataBase_Url
        {
            get {
                object regValue;
                if ((regValue = Key.GetValue("WSU_Database_Url")) != null)
                    return regValue.ToString();
                else
                    return null;
            }

            set {
                Key.SetValue("WSU_Database_Url", value, RegistryValueKind.String);
            }
        }

        private static RegistryKey Key
        {
            get { return Registry.CurrentUser.CreateSubKey(@"SOFTWARE\CustomerLogger"); }
        }
    }
}
