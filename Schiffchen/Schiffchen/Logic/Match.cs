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
    /// <summary>
    /// The class handles all ingame match logic and holds all data of the current game
    /// </summary>
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

        private Shot SendedShot;
        private Boolean GamestateSended;

        private Field selectedField;
        private IconButton btnAccept;
        private IconButton btnTurn;
        private Boolean DiceWinnerChecked;

        private int PartnersPreDice;
        private DispatcherTimer pingTimer;
   
        public FooterMenu FooterMenu { get; private set; }

        /// <summary>
        /// Creates a new match instance
        /// </summary>
        /// <param name="mid">The ID of the match</param>
        /// <param name="own">The own Jabber-ID</param>
        /// <param name="partner">The Jabber-ID of the game partner</param>
        public Match(String mid, JID own, JID partner)
        {
            this.MatchState = Enum.MatchState.ShipPlacement;
            this.PartnerDice = -1;
            this.OwnDice = -1;
            this.PartnersPreDice = -1;
            this.SendedShot = null;
            this.GamestateSended = false;
            this.MatchID = mid;
            this.OwnJID = own;
            this.PartnerJID = partner;
            this.Started = DateTime.Now;
            this.rnd = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        /// Initializes all complex objects. Because it handles content and textures, this method must not be called before the content
        /// of the static manager are loaded!
        /// </summary>
        public void Initialize()
        {
            this.OwnPlayground = new Playground(PlaygroundMode.Normal);
            this.ShootingPlayground = new Playground(PlaygroundMode.Minimap);
            this.ShootingPlayground.Click += new EventHandler<EventArgs>(shootingPlayground_Click);
            this.OwnPlayground.Click += new EventHandler<EventArgs>(OwnPlayground_Click);
            this.ShootingPlayground.TargetSelected += new EventHandler<ShootEventArgs>(ShootingPlayground_TargetSelected);
            AppCache.XmppManager.IncomingMatchPing += new EventHandler<MessageEventArgs>(XmppManager_IncomingPing);
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
            AppCache.XmppManager.IncomingGamestate += new EventHandler<MessageEventArgs>(XmppManager_IncomingGamestate);

            pingTimer = new DispatcherTimer();
            pingTimer.Interval = new TimeSpan(0, 0, 10);
            pingTimer.Tick += new EventHandler(pingTimer_Tick);
            pingTimer.Start();
        }

        /// <summary>
        /// Is called on receiving a message with the gamestate
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message event arguments</param>
        void XmppManager_IncomingGamestate(object sender, MessageEventArgs e)
        {
            if (!this.GamestateSended)
            {
                JID looser = getLooser();
                if (looser != null)
                {
                    Partner.SendGamestate(looser.BareJID);
                    this.GamestateSended = true;
                }                
            }
        }

        /// <summary>
        /// Iterates through all own and enemy ships and determines, if someone has won the game.        
        /// </summary>
        /// <returns>A JID object, which represents the looser. If it is null, there is no winner at the moment.</returns>
        private JID getLooser()
        {
            Boolean allOwnDestroyed = true;
            Boolean allPartnerDestroyed = true;
            foreach (Ship s in this.OwnShips)
            {
                if (s != null && !s.IsDestroyed)
                    allOwnDestroyed = false;
            }
            foreach (Ship s in this.PartnerShips)
            {
                if (s != null && !s.IsDestroyed)
                    allPartnerDestroyed = false;
            }
            if (allOwnDestroyed)
            {
                return OwnJID;
            }
            else if (allPartnerDestroyed)
            {
                return PartnerJID;
            }
            else
            {
                return null;
            }
        }
      
        /// <summary>
        /// Is called on receiving a message with a ping of the partner
        /// Sets the LastPing-Object to Now.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The message event arguments</param>
        void XmppManager_IncomingPing(object sender, MessageEventArgs e)
        {
            Partner.LastPing = DateTime.Now;
        }

        /// <summary>
        /// Is called, when the ping timer ticks.
        /// Determines, how the online state of the game partner should be displayed.
        /// Sends a ping message to the game partner.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        void pingTimer_Tick(object sender, EventArgs e)
        {            
            if (DateTime.Now - Partner.LastPing > new TimeSpan(0, 0, 40))
            {
                Partner.OnlineState = PartnerState.Offline;
            }
            else if (DateTime.Now - Partner.LastPing > new TimeSpan(0, 0, 15))
            {
                Partner.OnlineState = PartnerState.Waiting;
            }
            else
            {
                Partner.OnlineState = PartnerState.Online;
            }
            Partner.Ping();
        }

        /// <summary>
        /// Is called on receiving a message with a shot from the partner
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The shoot event arguments</param>
        void XmppManager_IncomingShot(object sender, ShootEventArgs e)
        {
            if (!this.IsMyTurn)
            {

                if (AppCache.CurrentMatch.OwnPlayground.fields[e.Y - 1, e.X - 1].ReferencedShip == null)
                {
                    AppCache.CurrentMatch.OwnPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Water;
                    SoundManager.SoundWater.Play();
                    Partner.TransferShotResult(e.X, e.Y, false, null);
                }
                else
                {
                    AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].FieldState = FieldState.Hit;
                    Ship sh = AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].ReferencedShip;
                    sh.HitOnField(AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1]);
                    SoundManager.SoundExplosion.Play();
                    ShipInfo shipInfo = null;
                    if (sh.IsDestroyed)
                    {
                        shipInfo = new ShipInfo();
                        shipInfo.X = sh.StartField.X;
                        shipInfo.Y = sh.StartField.Y;
                        shipInfo.Size = sh.Size;
                        shipInfo.Orientation = sh.Orientation;
                    }

                    Partner.TransferShotResult(e.X, e.Y, true, shipInfo);
                    //AppCache.CurrentMatch.ShootingPlayground.fields[e.Y - 1, e.X - 1].ReferencedShip.H
                }
                this.IsMyTurn = true;
                this.switchToTargetMode();

                // Lookup, if someone has won or lost
                JID looser = getLooser();
                if (looser != null && !GamestateSended)
                {
                    Partner.SendGamestate(looser.BareJID);
                    this.GamestateSended = true;
                }        
            }
        }

        void XmppManager_IncomingShotResult(object sender, ShootEventArgs e)
        {
            if (e.X == SendedShot.X && e.Y == SendedShot.Y)
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
            else
            {
                MessageBox.Show("Warning: Coordinates of sended shot and received shot result are different!");
            }
            this.SendedShot = null;
            FooterMenu.RemoveButton("btnAttack");
            this.IsMyTurn = false;
            this.switchToShipviewerMode();

            // Lookup, if someone has won or lost
            JID looser = getLooser();
            if (looser != null && !GamestateSended)
            {
                Partner.SendGamestate(looser.BareJID);
                this.GamestateSended = true;
            }    
        }


        void ShootingPlayground_TargetSelected(object sender, ShootEventArgs e)
        {
            if (SendedShot == null)
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
        }

        void attack_Click(object sender, EventArgs e)
        {
            if (this.SendedShot == null)
            {
                IconButton btnAttack = (IconButton)sender;
                btnAttack.Icon = TextureManager.IconAttackSW;
                Partner.Shoot(selectedField.X, selectedField.Y);
                this.SendedShot = new GameElemens.Shot(selectedField.X, selectedField.Y);
            }
        }

        void OwnPlayground_Click(object sender, EventArgs e)
        {
            switchToShipviewerMode();
        }

        void shootingPlayground_Click(object sender, EventArgs e)
        {
            switchToTargetMode();
        }

        private void switchToTargetMode()
        {
            if (this.MatchState == Enum.MatchState.Playing && this.ShootingPlayground.PlaygroundMode == PlaygroundMode.Minimap)
            {
                this.ShootingPlayground.IncreaseToMain();
                this.OwnPlayground.ReduceToMinimap();
            }
        }

        private void switchToShipviewerMode()
        {
            if (this.MatchState == Enum.MatchState.Playing && this.OwnPlayground.PlaygroundMode == PlaygroundMode.Minimap)
            {
                this.OwnPlayground.IncreaseToMain();
                this.ShootingPlayground.ReduceToMinimap();
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
                    this.switchToTargetMode();
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
