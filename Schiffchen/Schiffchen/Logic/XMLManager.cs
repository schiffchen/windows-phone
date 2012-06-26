using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net.XMPP;
using Schiffchen.Logic.Messages;

namespace Schiffchen.Logic
{
    /// <summary>
    /// Handles the logic of parsing XML messages
    /// </summary>
    public class XMLManager
    {
        /// <summary>
        /// Parse an XML string and returns a specific BattleshipMessage, if it was valid.
        /// </summary>
        /// <param name="strXML">The XML string</param>
        /// <returns>The specific BattleshipMessage. Returns null, if it was not a valid BattleshipMessage.</returns>
        public static BattleshipMessage GetBattleshipMessage(String strXML)
        {           
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(strXML)))
                {
                    Boolean isBattleship = false;
                    Boolean isShoot = false;

                    MatchMessage ShootMessage = null;

                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name.ToLower().Equals("battleship"))
                                {
                                    isBattleship = true;
                                }
                                else if ((isBattleship) && reader.Name.ToLower().Equals("queueing"))
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
                                else if ((isBattleship) && reader.Name.ToLower().Equals("diceroll"))
                                {
                                    int dice = Convert.ToInt32(reader.GetAttribute("dice"));
                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    dictionary.Add("dice", dice);
                                    MatchMessage message = new MatchMessage(Enum.MatchAction.Diceroll, dictionary);
                                    return message;
                                }
                                else if ((isBattleship) && reader.Name.ToLower().Equals("shoot"))
                                {
                                    isShoot = true;
                                    
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
                                    ShootMessage = message;
                                }
                                else if ((isShoot) && reader.Name.ToLower().Equals("ship"))
                                {
                                    if (ShootMessage != null)
                                    {
                                        ShipInfo shipInfo = new ShipInfo();
                                        shipInfo.X = Convert.ToInt32(reader.GetAttribute("x"));
                                        shipInfo.Y = Convert.ToInt32(reader.GetAttribute("y"));
                                        shipInfo.Destroyed = Convert.ToBoolean(reader.GetAttribute("destroyed"));
                                        shipInfo.Size = Convert.ToInt32(reader.GetAttribute("size"));
                                        String orientation = reader.GetAttribute("orientation");

                                        if (orientation.ToLower().Equals("horizontal"))
                                            shipInfo.Orientation = System.Windows.Controls.Orientation.Horizontal;
                                        else
                                            shipInfo.Orientation = System.Windows.Controls.Orientation.Vertical;

                                        ShootMessage.ShipInfo = shipInfo;                                        
                                    }
                                }
                                else if ((isBattleship) && reader.Name.ToLower().Equals("gamestate"))
                                {
                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    
                                    dictionary.Add("state", reader.GetAttribute("state"));
                                    dictionary.Add("looser", reader.GetAttribute("looser"));
                                    
                                    MatchMessage message = new MatchMessage(Enum.MatchAction.Gamestate, dictionary);
                                    return message;
                                }
                                else if ((isBattleship = true) && reader.Name.ToLower().Equals("ping"))
                                {
                                    Dictionary<String, Object> dictionary = new Dictionary<String, Object>();
                                    
                                    Enum.MatchAction action = Enum.MatchAction.Ping;

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
                                    if (isShoot && ShootMessage!=null)
                                    {
                                        return ShootMessage;
                                    }
                                }
                                
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
