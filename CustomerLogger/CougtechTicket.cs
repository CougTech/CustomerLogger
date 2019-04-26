using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLogger
{
    public class CougtechTicket
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        bool m_bIsAppointment, m_bIsQuickPick;
        string m_sNid, m_sCustomerName, m_sCustomerEmail, m_sDescription, m_sProblem, m_sSelf;

        //  Constructors    ///////////////////////////////////////////////////////////////////////

        public CougtechTicket(string sNid = "", string sCustomerName = "", string sCustomerEmail = "", string sProblem = "", string sDescription = "", bool bIsAppointment = false, bool bIsQuickPick = false)
        {
            m_bIsAppointment = bIsAppointment;
            m_bIsQuickPick = bIsQuickPick;
            m_sNid = sNid;
            m_sCustomerName = sCustomerName;
            m_sCustomerEmail = sCustomerEmail;
            m_sDescription = sDescription;
            m_sProblem = sProblem;
        }

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public bool IsAppointment
        {
            get { return m_bIsAppointment; }
            set { m_bIsAppointment = value; }
        }

        public bool IsQuickPick
        {
            get { return m_bIsQuickPick; }
            set { m_bIsQuickPick = value; }
        }

        public string Nid
        {
            get { return m_sNid; }
            set { m_sNid = value; }
        }

        public string CustomerName
        {
            get { return m_sCustomerName; }
            set { m_sCustomerName = value; }
        }

        public string CustomerEmail
        {
            get { return m_sCustomerEmail; }
            set { m_sCustomerEmail = value; }
        }

        public string Description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public string Problem
        {
            get { return m_sProblem; }
            set { m_sProblem = value; }
        }

        public string Self
        {
            get { return m_sSelf; }
            set { m_sSelf = value; }
        }
    }
}
