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
    public class MatchMessage : BattleshipMessage
    {
        public MatchAction Action;
        public JID JID;
        
        public Int32 MatchID;
        public Int32 Dice;
        public Int32 X;
        public Int32 Y;
        public String Result;

        public MatchMessage(MatchAction action, Dictionary<string, object> dict) : base(Type.Match)
        {
            this.Action = action;
            switch (action)
            {
                case MatchAction.diceroll:
                    this.Dice = (Int32)dict["dice"];
                    break;
                case MatchAction.shoot:
                    this.X = (Int32)dict["X"];
                    this.Y = (Int32)dict["Y"];
                    break;
                case MatchAction.shootresult:
                    this.X = (Int32)dict["X"];
                    this.Y = (Int32)dict["Y"];
                    this.Result = (String)dict["result"];
                    break;

            }
        }

        public String ToSendXML(JID from, JID to)
        {
            String s = "<message from=\"" + from.FullJID + "\" id=\"" + Guid.NewGuid() + "\" to=\"" + to.FullJID + "\" type=\"normal\">\n<battleship xmlns=\"http://battleship.me/xmlns/\">";
            switch (Action) {
                case MatchAction.diceroll:
                    s += "<diceroll dice=\"" + this.Dice + "\" />";
                        break; 
                case MatchAction.shoot:
                        s += "<shoot x=\"" + this.X + "\" y=\"" + this.Y + "\" />";
                        break;
                case MatchAction.shootresult:
                    s += "<shoot x=\"" + this.X + "\" y=\"" + this.Y + "\" result=\"" + this.Result + "\" />";
                        break;
                default:
                    throw new Exception("This Match Message Type is not for sending!");
            }
            s += "</battleship>\n</message>";
            return s;
        }
    }
}
