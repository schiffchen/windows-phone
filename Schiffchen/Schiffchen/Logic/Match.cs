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
using Schiffchen.GameElemens;
using Schiffchen.Event;
using Microsoft.Xna.Framework;
using System.Windows.Threading;

namespace Schiffchen.Logic
{
    public class Match
    {
        public String MatchID { get; private set; }
        public JID OwnJID { get; private set; }
        public JID PartnerJID { get; private set; }
        public DateTime Started { get; private set; }
        public Ship[] OwnShips { get; private set; }
        public Ship[] PartnerShips { get; private set; }
        public Boolean IsMyTurn { get; private set; }
        public Int32 OwnDice { get; private set; }
        public Int32 PartnerDice { get; set; }
        public MatchState MatchState { get; private set; }
        private Random rnd;
        public Playground OwnPlayground { get; private set; }
        public Playground ShootingPlayground { get; private set; }

        private Field selectedField;
        private IconButton btnAccept;
        private IconButton btnTurn;
        private Boolean DiceWinnerChecked;

        private int PartnersPreDice;
        private DispatcherTimer pingTimer;
   
        public FooterMenu FooterMenu { get; private set; }


        public Match(String mid, JID own, JID partner)
        {
            this.MatchState = Enum.MatchState.ShipPlacement;
            this.PartnerDice = -1;
            this.OwnDice = -1;
            this.PartnersPreDice = -1;

            this.MatchID = mid;
            this.OwnJID = own;
            this.PartnerJID = partner;
            this.Started = DateTime.Now;
            this.rnd = new Random(DateTime.Now.Millisecond);
        }

        public void Initialize()
        {
            this.OwnPlayground = new Playground(PlaygroundMode.Normal);
            this.ShootingPlayground = new Playground(PlaygroundMode.Minimap);
            this.ShootingPlayground.Click += new EventHandler<EventArgs>(shootingPlayground_Click);
            this.OwnPlayground.Click += new EventHandler<EventArgs>(OwnPlayground_Click);
            this.ShootingPlayground.TargetSelected += new EventHandler<ShootEventArgs>(ShootingPlayground_TargetSelected);
            AppCache.XmppManager.IncomingPing += new EventHandler<MessageEventArgs>(XmppManager_IncomingPing);
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

            AppCache.XmppManager.IncomingDiceroll += new EventHandler<RollingDiceEventArgs>(XmppManager_IncomingDiceroll);
            AppCache.XmppManager.IncomingShot += new EventHandler<ShootEventArgs>(XmppManager_IncomingShot);
            AppCache.XmppManager.IncomingShotResult += new EventHandler<ShootEventArgs>(XmppManager_IncomingShotResult);

            pingTimer = new DispatcherTimer();
            pingTimer.Interval = new TimeSpan(0, 0, 10);
            pingTimer.Tick += new EventHandler(pingTimer_Tick);
            pingTimer.Start();
        }

      
        void XmppManager_IncomingPing(object sender, MessageEventArgs e)
        {
            Partner.LastPing = DateTime.Now;
        }

        void pingTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - Partner.LastPing > new TimeSpan(0, 0, 15))
            {
                Partner.OnlineState = PartnerState.Waiting;
            }
            else if (DateTime.Now - Partner.LastPing > new TimeSpan(0, 0, 40))
            {
                Partner.OnlineState = PartnerState.Offline;
            }
            else
            {
                Partner.OnlineState = PartnerState.Online;
            }
            Partner.Ping();
        }

        void XmppManager_IncomingShot(object sender, ShootEventArgs e)
        {
            if (!this.IsMyTurn)
            {

                if (AppCache.CurrentMatch.OwnPlayground.fields[e.Y - 1, e.X - 1].ReferencedShip == null)
                {
                    AppCache.CurrentMatch.OwnPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Water;
                    SoundManager.SoundWater.Play();
                }
                else
                {
                    AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Hit;
                    SoundManager.SoundExplosion.Play();
                    //AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].ReferencedShip.H
                }
                MessageBox.Show("Incoming Shot: X=" + e.X + ", Y=" + e.Y);
                
            }
        }

        void XmppManager_IncomingShotResult(object sender, ShootEventArgs e)
        {
            if (e.Result.ToLower().Equals("water"))
            {
                SoundManager.SoundWater.Play();
                AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Water;
            }
            else
            {
                SoundManager.SoundExplosion.Play();
                AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Hit;
            }
        }


        void ShootingPlayground_TargetSelected(object sender, ShootEventArgs e)
        {
            Playground pgSender = (Playground)sender;
            pgSender.ResetFieldColors();
            Field selectedField = pgSender.fields[e.Y - 1, e.X - 1];
            selectedField.SetColor(FieldColor.Red);
            IconButton attack = new IconButton(TextureManager.IconAttack, "Attack", "btnAttack");
            attack.Click += new EventHandler<EventArgs>(attack_Click);
            FooterMenu.RemoveButton("btnAttack");
            FooterMenu.AddButton(attack);
            this.selectedField = selectedField;
        }

        void attack_Click(object sender, EventArgs e)
        {
            Partner.Shoot(selectedField.X, selectedField.Y);
            btnAccept.Visible = false;
        }

        void OwnPlayground_Click(object sender, EventArgs e)
        {
            if (this.MatchState == Enum.MatchState.Playing && this.OwnPlayground.PlaygroundMode == PlaygroundMode.Minimap)
            {
                this.OwnPlayground.IncreaseToMain();
                this.ShootingPlayground.ReduceToMinimap();
            }
        }

        void shootingPlayground_Click(object sender, EventArgs e)
        {
            if (this.MatchState == Enum.MatchState.Playing && this.ShootingPlayground.PlaygroundMode == PlaygroundMode.Minimap ) {
                this.ShootingPlayground.IncreaseToMain();
                this.OwnPlayground.ReduceToMinimap();
            }
        }

        void XmppManager_IncomingDiceroll(object sender, RollingDiceEventArgs e)
        {
            if (FooterMenu.Dices[1] != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                        {
                            FooterMenu.Dices[1].Roll(e.Value);
                        }
                );
            }
            else
            {
                this.PartnersPreDice = e.Value;               
            }
        }

        private void FakeIncomingDiceroll()
        {
            FooterMenu.Dices[1].Roll(1);
        }

        void CheckDiceWinner()
        {
            if (PartnerDice != -1 && OwnDice != -1)
            {
                if (PartnerDice > OwnDice)
                {
                    FooterMenu.Dices[1].Blink(TextureManager.Green);
                    FooterMenu.Dices[1].BlinkComplete +=new EventHandler<EventArgs>(Match_BlinkComplete);
                    DiceWinnerChecked = true;
                    this.IsMyTurn = false;
                }
                else if (PartnerDice < OwnDice)
                {
                    FooterMenu.Dices[0].Blink(TextureManager.Green);
                    FooterMenu.Dices[0].BlinkComplete += new EventHandler<EventArgs>(Match_BlinkComplete);
                    DiceWinnerChecked = true;
                    this.IsMyTurn = true;
                }
                else
                {
                    FooterMenu.Dices[0].Blink(TextureManager.Yellow);
                    FooterMenu.Dices[1].Blink(TextureManager.Yellow);
                    DispatcherTimer resetDices = new DispatcherTimer();
                    resetDices.Interval = new TimeSpan(0, 0, 2);
                    resetDices.Tick += new EventHandler(resetDices_Tick);
                    resetDices.Start();
                }
            }
        }

        void Match_BlinkComplete(object sender, EventArgs e)
        {
            this.FooterMenu.Dices[0] = null;
            this.FooterMenu.Dices[1] = null;
            this.MatchState = Enum.MatchState.Playing;
        }

        void resetDices_Tick(object sender, EventArgs e)
        {
            FooterMenu.Dices[0].ResetValue();
            FooterMenu.Dices[1].ResetValue();
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
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
                this.MatchState = Enum.MatchState.Dicing;
                FooterMenu.RemoveAllButtons();
                Dice ownDice = new GameElemens.Dice(new Vector2(20, 10), "Your Dice");
                ownDice.Click += new EventHandler<EventArgs>(ownDice_Click);
                FooterMenu.Dices[0] = ownDice;

                Dice partnersDice = new GameElemens.Dice(new Vector2(140, 10), "Partners Dice");
                partnersDice.ReadOnly = true;
                partnersDice.RollingFinish += new EventHandler<RollingDiceEventArgs>(partnersDice_RollingFinish);
                FooterMenu.Dices[1] = partnersDice;

                if (this.PartnersPreDice != -1)
                {
                    // Partner has alread rolled the dices
                    Deployment.Current.Dispatcher.BeginInvoke(delegate
                        {
                            FooterMenu.Dices[1].Roll(this.PartnersPreDice);
                        }
                );
                }

            }
        }

        void partnersDice_RollingFinish(object sender, RollingDiceEventArgs e)
        {
            this.PartnerDice = e.Value;
            CheckDiceWinner();
        }


        void ownDice_Click(object sender, EventArgs e)
        {
            Dice dice = (Dice)sender;
            dice.RollingFinish += new EventHandler<RollingDiceEventArgs>(dice_RollingFinish);
            dice.Roll();
            //this.FakeIncomingDiceroll();
            
        }

        void dice_RollingFinish(object sender, RollingDiceEventArgs e)
        {
            this.OwnDice = e.Value;
            Partner.Dice(e.Value);           
            CheckDiceWinner();
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
            if (this.MatchState == Enum.MatchState.Playing)
            {
                OwnPlayground.Update();
                ShootingPlayground.Update();
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            this.OwnPlayground.Draw(spriteBatch);
            this.ShootingPlayground.Draw(spriteBatch);
            FooterMenu.Draw(spriteBatch);
            foreach (Ship s in this.OwnShips)
            {
                s.Draw(spriteBatch);
            }
        }
    }
}
