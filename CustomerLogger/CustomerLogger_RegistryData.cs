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
