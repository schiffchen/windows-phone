﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Schiffchen.Logic.Enum;
using System.Net.XMPP;

namespace Schiffchen.Logic.Messages
{
    /// <summary>
    /// Defines a match message, which is sent or received over XMPP
    /// </summary>
    public class MatchMessage : BattleshipMessage
    {
        public MatchAction Action;
        public JID JID;

        public Int32 MatchID;
        public Int32 Dice;
        public Int32 X;
        public Int32 Y;
        public String Result;

        public String State;
        public String Looser;

        public ShipInfo ShipInfo;

        /// <summary>
        /// Creates a new entity of the MatchMessage
        /// </summary>
        /// <param name="action">The current action</param>
        /// <param name="dict">The dictionary with parameters</param>
        public MatchMessage(MatchAction action, Dictionary<string, object> dict)
            : base(Schiffchen.Logic.Enum.Type.Match)
        {
            this.Action = action;
            switch (action)
            {
                case MatchAction.Diceroll:
                    this.Dice = (Int32)dict["dice"];
                    break;
                case MatchAction.Shot:
                    this.X = (Int32)dict["x"];
                    this.Y = (Int32)dict["y"];
                    break;
                case MatchAction.Shotresult:
                    this.X = (Int32)dict["x"];
                    this.Y = (Int32)dict["y"];
                    this.Result = (String)dict["result"];
                    break;
                case MatchAction.Gamestate:
                    this.Looser = (String)dict["looser"];
                    this.State = (String)dict["state"];
                    break;

            }
        }

        /// <summary>
        /// Generates the XML for sending over XMPP
        /// </summary>
        /// <returns>An XML string</returns>
        public String ToSendXML()
        {
            return this.ToSendXML(this.From, this.To);
        }

        /// <summary>
        /// Generates the XML for sending over XMPP
        /// </summary>
        /// <param name="from">The Jabber-ID of the sender</param>
        /// <param name="to">The Jabber-ID of the receiver</param>
        /// <returns>An XML string</returns>
        public String ToSendXML(JID from, JID to)
        {
            String s = "<message from=\"" + from.FullJID + "\" id=\"" + Guid.NewGuid() + "\" to=\"" + to.FullJID + "\" type=\"normal\">\n<battleship xmlns=\"http://battleship.me/xmlns/\">";
            switch (Action)
            {
                case MatchAction.Diceroll:
                    s += "<diceroll dice=\"" + this.Dice + "\" />";
                    break;
                case MatchAction.Shot:
                    s += "<shoot x=\"" + this.X + "\" y=\"" + this.Y + "\" />";
                    break;
                case MatchAction.Shotresult:
                    s += "<shoot x=\"" + this.X + "\" y=\"" + this.Y + "\" result=\"" + this.Result + "\" />";
                    if (this.ShipInfo != null)
                    {
                        s += "<ship x=\"" + this.ShipInfo.X + "\" y=\"" + this.ShipInfo.Y + "\" size=\"" + this.ShipInfo.Size + "\" orientation=\"" + this.ShipInfo.Orientation.ToString().ToLower() + "\" destroyed=\"" + this.ShipInfo.Destroyed.ToString().ToLower() + "\" />";
                    }
                    break;
                case MatchAction.Ping:
                    s += "<ping />";
                    break;
                case MatchAction.Gamestate:
                    s += "<gamestate state=\"" + this.State + "\" looser=\"" + this.Looser + "\" />";
                    break;
                default:
                    throw new Exception("This Match Message Type is not for sending!");
            }
            s += "</battleship>\n</message>";
            return s;
        }
    }
}
