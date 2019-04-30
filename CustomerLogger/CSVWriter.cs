using System.IO;

namespace CSV 
{
    /// <summary>
    /// Handler for the logger file
    /// </summary>
    public class CSVWriter 
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private string m_sLine, m_sFileName;

        private FileStream m_fStream;
        private StreamWriter m_sWriter;

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Custom Constructor
        /// </summary>
        /// <param name="sFileName">CSV File to write to</param>
        /// <param name="mode">How to initially handle file, Create will delete an existing file. Append will not.</param>
        public CSVWriter(string sFileName, FileMode mode)
        {
            m_sLine = "";
            m_sFileName = sFileName;

            //If the file mode is Create
            if (mode == FileMode.Create)
            {
                if (File.Exists(m_sFileName))   //If the file exists
                    File.Delete(m_sFileName);       //Delete the file
            }
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Appends a string to the end of the current line as a new CSV cell
        /// </summary>
        /// <param name="sText">String to append.</param>
        public void AddToCurrent(string sText)
        {
            if (m_sLine == "")                      //If the line is blank
                m_sLine = sText;                         //Add the string to the beginnging
            else                                    //Else
                m_sLine += ("," + sText);         //Append the string to the end of the line as a new CSV cell
        }

        /// <summary>
        /// Appends a string to the beginning of the current line as a new CSV cell
        /// </summary>
        /// <param name="sText">String to append.</param>
        public void AddToStart(string sText)
        {
            if (m_sLine == "")
                m_sLine = sText;
            else
                m_sLine = sText + "," + m_sLine;
        }

        /// <summary>
        /// Opens the logger file and appends the current line. Closes the file when finished.
        /// </summary>
        /// <remarks>
        /// The file opened is specified by m_sFileName. The line appended is m_sLine.
        /// </remarks>
        public void WriteLine()
        {
            //Create filestream and write line to the file
            using (m_fStream = new FileStream(m_sFileName, FileMode.Append))
            {
                using (m_sWriter = new StreamWriter(m_fStream))
                    m_sWriter.WriteLine(m_sLine);
            }

            m_sLine = "";
        }
    }
}
