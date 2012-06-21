using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace System.Net.XMPP
{

    [XmlRoot(ElementName = "query")]
    public class RegisterQuery
    {
        public RegisterQuery()
        {
        }

        private string m_strUserName = null;
        [XmlElement(ElementName = "username")]
        public string UserName
        {
            get { return m_strUserName; }
            set { m_strUserName = value; }
        }

        private string m_strPassword = null;
        [XmlElement(ElementName = "password")]
        public string Password
        {
            get { return m_strPassword; }
            set { m_strPassword = value; }
        }


        private string m_strEmail = null;
        [XmlElement(ElementName = "email")]
        public string Email
        {
            get { return m_strEmail; }
            set { m_strEmail = value; }
        }
    }


    [XmlRoot(ElementName = "iq")]
    public class RegisterQueryIQ : IQ
    {
         public RegisterQueryIQ()
            : base()
        {
        }
         public RegisterQueryIQ(string strXML)
            : base(strXML)
        {
        }

        [XmlElement(ElementName = "query", Namespace = "jabber:iq:register")]
        public RegisterQuery RegisterQuery = new RegisterQuery();

    }
}
