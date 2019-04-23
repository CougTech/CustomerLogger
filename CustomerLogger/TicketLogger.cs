using CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace CustomerLogger
{
    public class TicketLogger
    {
        private string m_sLogDirPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private CSVWriter m_CsvLogger;

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        public TicketLogger()
        {
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Calls logger procedure to append a string of text to the end of the line.
        /// </summary>
        /// <param name="sText">String to append.</param>
        public void AppendToLog(string sText)
        {
            if (m_CsvLogger != null)
                m_CsvLogger.AddToCurrent(sText);
        }

        /// <summary>
        /// Writes the closing time to the logger file and deallocates the logger.
        /// </summary>
        public void CloseLog()
        {
            if (m_CsvLogger != null)
            {
                m_CsvLogger.AddToCurrent("Log End Time: ");
                m_CsvLogger.AddToCurrent(DateTime.Now.ToShortTimeString());
                m_CsvLogger.WriteLine();

                //Close and dealocate the CSV writer
                m_CsvLogger = null;
            }
        }

        /// <summary>
        /// Creates a new CSVWriter object which will write to a new file.
        /// </summary>
        /// <param name="sFileName">name of csv file.</param>
        public void GenerateLogFile(string sFileName, FileMode mode)
        {
            //Ceate a new CSV file
            try
            {
                if (mode == FileMode.Create)
                {
                    int i = 0;
                    string sLogName = m_sLogDirPath + "\\Cougtech_Customer_Logs\\" + sFileName + ".csv";

                    //This loop guarantees that it will not overwrite the file, but instead append a number to the end and create a new file
                    while (File.Exists(sLogName))
                    {
                        i += 1;
                        sLogName = m_sLogDirPath + "\\" + sFileName + "_" + i.ToString() + ".csv";
                    }

                    m_CsvLogger = new CSVWriter(sLogName, mode);

                    //Headers for the log file
                    //The name of the log, the time it started
                    m_CsvLogger.AddToCurrent("Coutech customer log for the date of: ");
                    m_CsvLogger.AddToCurrent(DateTime.Now.ToString("MM-dd-yyyy"));
                    m_CsvLogger.WriteLine();
                    m_CsvLogger.AddToCurrent("Log start time: ");
                    m_CsvLogger.AddToCurrent(DateTime.Now.ToShortTimeString());
                    m_CsvLogger.WriteLine();
                    //Now the column headers
                    m_CsvLogger.AddToCurrent("Time");
                    m_CsvLogger.AddToCurrent("Type");
                    m_CsvLogger.AddToCurrent("URL");
                    m_CsvLogger.AddToCurrent("ID Number");
                    m_CsvLogger.AddToCurrent("Problem");
                    m_CsvLogger.AddToCurrent("Description");
                    m_CsvLogger.WriteLine();
                }
                else if (mode == FileMode.Append)
                    m_CsvLogger = new CSVWriter(sFileName, mode);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, $"Error generating log file + \"{sFileName}\"");
                return;
            }
        }

        /// <summary>
        /// Checks for whether the CSV Logger is initialized.
        /// </summary>
        /// <returns>True if the CSV logger is instantiated. False if it is null.</returns>
        public bool IsEnabled()
        {
            return (m_CsvLogger != null);
        }

        /// <summary>
        /// Enters a completed customer ticket into the current log file.
        /// </summary>
        /// <param name="customerTicket">Populated customer ticket.</param>
        public void LogTicket(CougtechTicket customerTicket)
        {
            m_CsvLogger.AddToCurrent(DateTime.Now.ToShortTimeString()); //1st column is the time that the ticket was logged

            //2nd column is the ticket type
            if (customerTicket.IsAppointment)           //If the ticket is an appointment
                m_CsvLogger.AddToCurrent("Apt");            //Set the type to "Apt"
            else if (customerTicket.IsQuickPick)        //Else if the ticket is a quick-pick
                m_CsvLogger.AddToCurrent("QickPick");       //Set the type to "QuickPick"
            else                                        //Else
                m_CsvLogger.AddToCurrent("WI");             //Set the type to "WI" for walk-in

            m_CsvLogger.AddToCurrent(customerTicket.SelfUrl);           //2nd column is the url to the ticket within Jira
            m_CsvLogger.AddToCurrent(customerTicket.Nid);               //3rd column is the NID of the student
            m_CsvLogger.AddToCurrent(customerTicket.Problem);           //4th column is the walk-in problem
            m_CsvLogger.AddToCurrent(customerTicket.Description);       //5th column is the problem description
            m_CsvLogger.WriteLine();                                    //Write the line to the log file
        }
    }
}
