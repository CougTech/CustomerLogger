using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira_REST
{
    public class JiraRestResponseBody
    {
        private string m_sID, m_sKey, m_sSelf;

        public string id
        {
            get { return m_sID; }
            set { m_sID = value; }
        }

        public string key
        {
            get { return m_sKey; }
            set { m_sKey = value; }
        }

        public string self
        {
            get { return m_sSelf; }
            set { m_sSelf = value; }
        }
    }

}
