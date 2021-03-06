﻿/// Copyright (c) 2011 Brian Bonnett
/// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
/// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

using System;
using System.Net;


using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace System.Net.XMPP
{
    public class GenericIQLogic : Logic
    {
        public GenericIQLogic(XMPPClient client)
            : base(client)
        {
            BindIQ = new IQ();
            BindIQ.Type = IQType.set.ToString();
            BindIQ.To = null;
            BindIQ.From = null;
        }

        private string m_strInnerXML = "";

        public string InnerXML
        {
            get { return m_strInnerXML; }
            set { m_strInnerXML = value; }
        }

        public override void Start()
        {
            base.Start();
            Bind();
        }

        public const string BindXML = @"<bind xmlns=""urn:ietf:params:xml:ns:xmpp-bind""><resource>##RESOURCE##</resource></bind>";
        
        IQ BindIQ = null;

        void Bind()
        {
            BindIQ.InnerXML = BindXML.Replace("##RESOURCE##", XMPPClient.JID.Resource);

            XMPPClient.XMPPState = XMPPState.Binding;
            XMPPClient.SendXMPP(BindIQ);
        }

        IQ sessioniq = null;
        internal void StartSession()
        {
            sessioniq = new IQ();
            sessioniq.InnerXML = "<session xmlns='urn:ietf:params:xml:ns:xmpp-session'/>";
            sessioniq.From = null;
            sessioniq.To = null;
            sessioniq.Type = IQType.set.ToString();
            XMPPClient.SendXMPP(sessioniq);
        }

        
        public override bool NewIQ(IQ iq)
        {
            try
            {
                if ( (BindIQ != null) && (iq.ID == BindIQ.ID))
                {
                    /// Extract our jid incase it changed
                    /// <iq type="result" id="bind_1" to="ninethumbs.com/7b5005e1"><bind xmlns="urn:ietf:params:xml:ns:xmpp-bind"><jid>test@ninethumbs.com/hypnotoad</jid></bind></iq>
                    /// 
                    if (iq.Type == IQType.result.ToString())
                    {
                        /// bound, now do toher things
                        /// 
                        XElement elembind = XElement.Parse(iq.InnerXML);
                        XElement nodejid = elembind.FirstNode as XElement;
                        if ((nodejid != null) && (nodejid.Name == "{urn:ietf:params:xml:ns:xmpp-bind}jid"))
                        {
                            XMPPClient.JID = nodejid.Value;
                        }
                        XMPPClient.XMPPState = XMPPState.Bound;
                    }
                    return true;
                }
                else if ((sessioniq != null) && (iq.ID == sessioniq.ID))
                {
                    XMPPClient.XMPPState = XMPPState.Session;
                    return true;
                }

                if ((iq.InnerXML != null) && (iq.InnerXML.Length > 0))
                {

                    XElement elem = XElement.Parse(iq.InnerXML);
                    if (elem.Name == "{urn:xmpp:ping}ping")
                    {
                        iq.Type = IQType.result.ToString();
                        iq.To = iq.From;
                        iq.From = XMPPClient.JID.BareJID;
                        iq.InnerXML = "";
                        XMPPClient.SendXMPP(iq);
                    }

                }
            }
            catch (Exception)
            {
            }
            return false;
        }

    }

    /// <summary>
    /// The method that is used to get xml from the object
    /// </summary>
    public enum SerializationMethod
    {
        /// <summary>
        /// Use the XMLSerializer to get xml from the object
        /// </summary>
        XMLSerializeObject,

        /// <summary>
        /// Use the virtual MessageXML property to xml from the object
        /// </summary>
        MessageXMLProperty,
    }

    public class SendRecvIQLogic : WaitableLogic
    {
        public SendRecvIQLogic(XMPPClient client, IQ iq)
            : base(client)
        {
            SendIQ = iq;
        }

        private string m_strInnerXML = "";

        public string InnerXML
        {
            get { return m_strInnerXML; }
            set { m_strInnerXML = value; }
        }

     
        public bool SendReceive(int nTimeoutMs)
        {
            if (SerializationMethod == XMPP.SerializationMethod.MessageXMLProperty)
                XMPPClient.SendXMPP(SendIQ);
            else
                XMPPClient.SendObject(SendIQ);

            Success = GotEvent.WaitOne(nTimeoutMs);
            return Success;
        }


        IQ m_objSendIQ = null;
        public IQ SendIQ
        {
            get { return m_objSendIQ; }
            set { m_objSendIQ = value; }
        }

        private IQ m_objRecvIQ = null;
        public IQ RecvIQ
        {
            get { return m_objRecvIQ; }
            set { m_objRecvIQ = value; }
        }

        public override bool NewIQ(IQ iq)
        {
            try
            {
                if (iq.ID == SendIQ.ID)
                {
                    RecvIQ = iq;
                    IsCompleted = true;
                    Success = true;
                    GotEvent.Set();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

    }


    public class WaitForMessageLogic : WaitableLogic
    {
        public WaitForMessageLogic(XMPPClient client, Type msgtype)
            : base(client)
        {
            MessageType = msgtype;
        }

        Type MessageType = null;

        private string m_strInnerXML = "";

        public string InnerXML
        {
            get { return m_strInnerXML; }
            set { m_strInnerXML = value; }
        }


        private Message m_objRecvMessage = null;

        public Message RecvMessage
        {
            get { return m_objRecvMessage; }
            set { m_objRecvMessage = value; }
        }

        public override bool NewMessage(Message iq)
        {
            try
            {
                if (iq.GetType() ==  MessageType)
                {
                    RecvMessage = iq;
                    IsCompleted = true;
                    Success = true;
                    GotEvent.Set();
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }


    }
}
