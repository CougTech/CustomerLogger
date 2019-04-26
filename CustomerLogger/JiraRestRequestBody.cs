using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira_REST
{
    /// <summary>
    /// 
    /// </summary>
    public class JiraRestRequestBody
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private Fields m_Fields;

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public Fields fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        //  Public Functions    ///////////////////////////////////////////////////////////////////

        public void Populate(string sSummary, string m_sDescription)
        {
            m_Fields = new Fields {
                summary = sSummary,
                description = m_sDescription,
                issuetype = new Issuetype() 
                {
                    name = "IT Help"
                },
                project = new Project 
                {
                    key = "CTWI"
                }
            };
            m_Fields.components[0] = new Component() {
                name = "RestService"
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Fields
    {
        //  Members ///////////////////////////////////////////////////////////////////////////////

        private string m_sSummary, m_sDescription;

        private Issuetype m_IssueType;
        private Project m_Project;
        private Component[] m_Components = new Component[1];

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public Project project
        {
            get { return m_Project; }
            set { m_Project = value; }
        }

        public string summary
        {
            get { return m_sSummary; }
            set { m_sSummary = value; }
        }

        public string description
        {
            get { return m_sDescription; }
            set { m_sDescription = value; }
        }

        public Issuetype issuetype
        {
            get { return m_IssueType; }
            set { m_IssueType = value; }
        }

        public Component[] components
        {
            get { return m_Components; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Project
    {
        //  Public Members  ///////////////////////////////////////////////////////////////////////

        private string m_sKey;

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string key
        {
            get { return m_sKey; }
            set { m_sKey = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Issuetype
    {
        //  Public Members  ///////////////////////////////////////////////////////////////////////

        private string m_sName;

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Component
    {
        //  Public Members  ///////////////////////////////////////////////////////////////////////

        private string m_sName;

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }
    }

}
