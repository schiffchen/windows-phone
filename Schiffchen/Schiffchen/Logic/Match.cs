using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using System.Net.XMPP;
using Schiffchen.GameElemens;

namespace Schiffchen.Logic
{
    public class Match
    {
        public Int32 MatchID { get; private set; }
        public JID OwnJID { get; private set; }
        public JID PartnerJID { get; private set; }
        public DateTime Started { get; private set; }
        public Ship[] OwnShips { get; private set; }
        public Ship[] PartnerShips { get; private set; }
        public JID CurrentTurn { get; private set; }
        public Int32 OwnDice { get; private set; }
        public Int32 PartnerDice { get; set; }
        private Random rnd;

        private Playground currentPlayground;

        public Match(Int32 mid, JID own, JID partner)
        {
            this.MatchID = mid;
            this.OwnJID = own;
            this.PartnerJID = partner;
            this.Started = DateTime.Now;
            this.rnd = new Random(DateTime.Now.Millisecond);
            this.currentPlayground = new Playground();
            InitializeShips();
        }

        private void InitializeShips()
        {
            this.OwnShips = new Ship[4];
            this.PartnerShips = new Ship[4];
            this.OwnShips[0] = new Ship(this.OwnJID, ShipType.DESTROYER);
            this.OwnShips[1] = new Ship(this.OwnJID, ShipType.SUBMARINE);
            this.OwnShips[2] = new Ship(this.OwnJID, ShipType.BATTLESHIP);
            this.OwnShips[3] = new Ship(this.OwnJID, ShipType.AIRCRAFT_CARRIER);

            /*
            this.PartnerShips[0] = new Ship(this.PartnerJID, ShipType.DESTROYER, System.Windows.Controls.Orientation.Vertical, currentPlayground.fields[0, 4]);
            this.PartnerShips[1] = new Ship(this.PartnerJID, ShipType.SUBMARINE, System.Windows.Controls.Orientation.Vertical, currentPlayground.fields[0, 4]);
            this.PartnerShips[2] = new Ship(this.PartnerJID, ShipType.BATTLESHIP, System.Windows.Controls.Orientation.Vertical, currentPlayground.fields[0, 4]);
            this.PartnerShips[3] = new Ship(this.PartnerJID, ShipType.AIRCRAFT_CARRIER, System.Windows.Controls.Orientation.Vertical, currentPlayground.fields[0, 4]);
             * 
             * */
        }

        public Int32 Dice()
        {
            this.OwnDice = rnd.Next(1, 7);
            return this.OwnDice;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.currentPlayground.Draw(spriteBatch);
            foreach (Ship s in this.OwnShips)
            {
                s.Draw(spriteBatch);
            }
        }
    }
}
