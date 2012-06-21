using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Net.XMPP;
using Schiffchen.GameElemens;
using Schiffchen.Logic.Enum;
using Schiffchen.Resources;
using Schiffchen.Controls;
using Schiffchen.Logic.Messages;

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
        public MatchState MatchState { get; private set; }
        private Random rnd;

        private Playground currentPlayground;
        private IconButton btnAccept;
        private IconButton btnTurn;
   
        public FooterMenu FooterMenu { get; private set; }

        public Playground Playground { get { return this.currentPlayground; } }

        public Match(Int32 mid, JID own, JID partner)
        {
            this.MatchState = Enum.MatchState.ShipPlacement;
                        

            this.MatchID = mid;
            this.OwnJID = own;
            this.PartnerJID = partner;
            this.Started = DateTime.Now;
            this.rnd = new Random(DateTime.Now.Millisecond);
            this.currentPlayground = new Playground();
            InitializeShips();
            FooterMenu = new FooterMenu(DeviceCache.BelowGrid, DeviceCache.ScreenHeight - DeviceCache.BelowGrid);

            btnAccept = new IconButton(TextureManager.IconAccept, "Place", "btnPlace");
            btnTurn = new IconButton(TextureManager.IconTurn, "Turn", "btnTurn");
           
            btnAccept.Visible = false;
            btnTurn.Visible = false;

            FooterMenu.AddButton(btnAccept);
            FooterMenu.AddButton(btnTurn);

            btnTurn.Click += new EventHandler<EventArgs>(btnTurn_Click);
            btnAccept.Click += new EventHandler<EventArgs>(btnAccept_Click);
        }

        void btnTurn_Click(object sender, EventArgs e)
        {
            if (AppCache.ActivePlacementShip != null)
            {
                AppCache.ActivePlacementShip.ToggleOrientation();
            }
        }

        void btnAccept_Click(object sender, EventArgs e)
        {
            if (AppCache.ActivePlacementShip != null)
            {
                AppCache.ActivePlacementShip.FinishPlacement();
            }
            if (areAllShipsPlaced())
            {
                FooterMenu.RemoveAllButtons();
                IconButton btnDice = new IconButton(TextureManager.IconAccept, "Roll Dice", "btnDice");
                btnDice.Click += new EventHandler<EventArgs>(btnDice_Click);
                FooterMenu.AddButton(btnDice);
            }
        }

        void btnDice_Click(object sender, EventArgs e)
        {
            int dice = this.Dice();
            
        }

        private void SendDiceMessage(int dice)
        {
            Dictionary<string,object> dict = new Dictionary<string,object>();
            dict.Add("dice", dice);
            MatchMessage message = new MatchMessage(MatchAction.diceroll, dict);
            AppCache.XmppManager.Client.SendRawXML(message.ToSendXML(this.OwnJID, this.PartnerJID));
        }

        private Boolean areAllShipsPlaced()
        {
            foreach (Ship s in this.OwnShips)
            {
                if (!s.IsPlaced)
                    return false;
            }
            return true;
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

        public void Update()
        {           
            if (this.MatchState == Enum.MatchState.ShipPlacement)
            {
                if (AppCache.ActivePlacementShip != null)
                {
                    btnTurn.Visible = true;
                }
                else
                {
                    btnAccept.Visible = false;
                    btnTurn.Visible = false;
                }

                if (this.OwnShips[0].IsPlaced && this.OwnShips[1].IsPlaced && this.OwnShips[2].IsPlaced && this.OwnShips[0].IsPlaced)
                {
                    // The placement of ships is finished. Switch to the next match state
                    this.MatchState = Enum.MatchState.Dicing;
                }
            }
        }

        public Int32 Dice()
        {
            this.OwnDice = rnd.Next(1, 7);
            return this.OwnDice;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            this.currentPlayground.Draw(spriteBatch);
            FooterMenu.Draw(spriteBatch);
            foreach (Ship s in this.OwnShips)
            {
                s.Draw(spriteBatch);
            }
        }
    }
}
