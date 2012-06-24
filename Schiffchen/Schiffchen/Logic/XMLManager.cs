using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.XMPP;
using Schiffchen.Logic.Messages;

namespace Schiffchen.Logic
{
    public class XMLManager
    {
        public static BattleshipMessage GetBattleshipMessage(String strXML)
        {           
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(strXML)))
                {
                    Boolean isBattleship = false;

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.ToLower().Equals("battleship"))
                                {
                                    isBattleship = true;
                                }
                                else if ((isBattleship = true) && reader.Name.ToLower().Equals("queueing"))
                                {
                                    String result = reader.GetAttribute("action");

                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    QueuingMessage message = null;
                                    if (result != null)
                                    {
                                        switch (result)
                                        {
                                            case "request":
                                                message = new QueuingMessage(Enum.QueueingAction.request, null);
                                                break;
                                            case "success":
                                                dictionary.Add("id", Convert.ToInt32(reader.GetAttribute("id")));
                                                message = new QueuingMessage(Enum.QueueingAction.success, dictionary);
                                                break;
                                            case "ping":
                                                string id = reader.GetAttribute("id");
                                                if (!String.IsNullOrEmpty(id))
                                                {
                                                    dictionary.Add("id", Convert.ToInt32(id));
                                                }
                                                message = new QueuingMessage(Enum.QueueingAction.ping, dictionary);
                                                break;
                                            case "assign":
                                                dictionary.Add("jid", new JID(reader.GetAttribute("jid")));
                                                dictionary.Add("mid", reader.GetAttribute("mid"));
                                                message = new QueuingMessage(Enum.QueueingAction.assign, dictionary);
                                                break;
                                            case "assigned":
                                                dictionary.Add("jid", new JID(reader.GetAttribute("jid")));
                                                dictionary.Add("mid", Convert.ToInt32(reader.GetAttribute("mid")));
                                                message = new QueuingMessage(Enum.QueueingAction.assigned, dictionary);
                                                break;
                                        }
                                    }                                 
                                    return message;
                                }
                                else if ((isBattleship = true) && reader.Name.ToLower().Equals("diceroll"))
                                {
                                    int dice = Convert.ToInt32(reader.GetAttribute("dice"));
                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    dictionary.Add("dice", dice);
                                    MatchMessage message = new MatchMessage(Enum.MatchAction.Diceroll, dictionary);
                                    return message;
                                }
                                else if ((isBattleship = true) && reader.Name.ToLower().Equals("shoot"))
                                {
                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    int x = Convert.ToInt32(reader.GetAttribute("x"));
                                    int y = Convert.ToInt32(reader.GetAttribute("y"));
                                    string result = reader.GetAttribute("result");

                                    dictionary.Add("x", x);
                                    dictionary.Add("y", y);
                                    Enum.MatchAction action = Enum.MatchAction.Shot;

                                    if (!String.IsNullOrEmpty(result)) {
                                        action = Enum.MatchAction.Shotresult;
                                        dictionary.Add("result", result);
                                    }
                                    
                                    MatchMessage message = new MatchMessage(action, dictionary);
                                    return message;
                                }
                                break;
                            case XmlNodeType.Text:

                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:

                                break;
                            case XmlNodeType.Comment:
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name.ToLower().Equals("battleship"))
                                {
                                    isBattleship = false;
                                }
                                else if ((isBattleship = true) && reader.Name.ToLower().Equals("queueing"))
                                {

                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }
}
