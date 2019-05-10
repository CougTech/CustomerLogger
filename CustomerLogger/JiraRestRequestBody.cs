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

        /// <summary>
        /// Populates the REST request body with new ticket information.
        /// </summary>
        /// <param name="sSummary">Summary for the ticket.</param>
        /// <param name="sDescription">Description of the ticket.</param>
        /// <param name="sEmailAddress">Email address of the ticket reporter</param>
        public void Populate(string sSummary, string sDescription, string sEmailAddress)
        {
            m_Fields = new Fields {
                summary = sSummary,
                description = sDescription,
                issuetype = new Issuetype() 
                {
                    name = "IT Help"
                },
                project = new Project 
                {
                    key = "CTWI"
                },
                reporter = new Reporter()
                {
                    emailAddress = sEmailAddress
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
        private Reporter m_Reporter;

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
            set { m_Components = value; }
        }

        public Reporter reporter
        {
            get { return m_Reporter; }
            set { m_Reporter = value; }
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

    public class Reporter
    {
        //  Public Members  ///////////////////////////////////////////////////////////////////////

        private string m_sEmailAddress, m_sName;

        //  Properties  ///////////////////////////////////////////////////////////////////////////

        public string emailAddress
        {
            get { return m_sEmailAddress; }
            set { m_sEmailAddress = value; }
        }

        public string name
        {
            get { return m_sName; }
            set { m_sName = value; }
        }
    }
}
