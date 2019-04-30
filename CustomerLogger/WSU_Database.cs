using System;
using System.Xml;

namespace WSU_Database
{
    public class Wsu_Database
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private const string m_sIdLookupUrl = "https://itsforms.wsu.edu/cougtech/Look.aspx?idn=";

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Uses a WSU NID to query the database for that user's first name.
        /// </summary>
        /// <remarks>
        /// If the response from the database does not contain an email address, null will be returned.
        /// </remarks>
        /// <param name="sNid">WSU NID.</param>
        /// <returns>User's first name if the NID exists within the database. Null if not.</returns>
        public static string Get_FirstName(string sNid)
        {
            string sResult = null;
            string sURLString = m_sIdLookupUrl + sNid;

            XmlTextReader xReader = new XmlTextReader(sURLString);

            while (xReader.Read())
            {
                if (xReader.NodeType == XmlNodeType.Element)
                {
                    if (xReader.Name == "FirstName")
                    {
                        xReader.Read();
                        sResult = xReader.Value;
                        return sResult;
                    }
                }
            }

            return sResult;
        }

        /// <summary>
        /// Uses a WSU NID to query the database for that user's email address.
        /// </summary>
        /// <remarks>
        /// If the response from the database does not contain an email address, "cougtech@wsu.edu" will be returned.
        /// </remarks>
        /// <param name="sNid">WSU NID.</param>
        /// <returns>User's email address if the NID exists within the database. "cougtech@wsu.edu" if not.</returns>
        public static string Get_WsuEmail(string sNid)
        {
            string sURLString = m_sIdLookupUrl + sNid;

            //Read the response from the URL
            XmlTextReader xReader = new XmlTextReader(sURLString);

            while (xReader.Read())
            {
                switch (xReader.NodeType)
                {
                    case XmlNodeType.Element:   //The response is an element.

                        Console.Write("<" + xReader.Name);

                        //Run through all attributes
                        while (xReader.MoveToNextAttribute())
                            Console.Write(" " + xReader.Name + "='" + xReader.Value + "'");

                        Console.Write(">");
                        Console.WriteLine(">");
                        break;

                    case XmlNodeType.Text:  //The response is a string

                        string sTemp = xReader.Value;
                        Console.WriteLine(sTemp);
                        if (sTemp.Contains("@"))   //If the string is an email
                            return sTemp;           //Return the string

                        break;

                    case XmlNodeType.EndElement:    //The response is the end of the element

                        Console.Write("</" + xReader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }

            return "cougtech@wsu.edu";
        }
    }
}
