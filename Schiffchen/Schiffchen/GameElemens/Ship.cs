using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net.XMPP;
using Schiffchen.Resources;
using Schiffchen.Logic;
using Schiffchen.Logic.Enum;

namespace Schiffchen.GameElemens
{
    /// <summary>
    /// Represents a single ship
    /// </summary>
    public class Ship
    {
        public ShipType ShipType { get; private set; }
        public Vector2 Position { get; set; }
        public JID Owner { get; private set; }
        public Dictionary<int, bool> HitPoints { get; set; }
        public Boolean IsDestroyed { get; set; }
        public Int32 Size { get; private set; }
        public System.Windows.Controls.Orientation Orientation;
        public Field StartField { get; private set; }
        public Boolean IsPlaced { get; private set; }

        private List<Field> Fields;

        public Boolean isTouched { get; set; }

        public Color OverlayColor { get; set; }
        

        private Texture2D shipTexture;
        private float scaleRate;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(shipTexture.Width * scaleRate), Convert.ToInt32(shipTexture.Height * scaleRate));
            }
        }

        /// <summary>
        /// Creates a new instance of a ship
        /// </summary>
        /// <param name="owner">The Jabber-ID of the owner</param>
        /// <param name="type">The type</param>
        /// <param name="or">The orientation</param>
        /// <param name="targetField">The target field, where the ship is placed</param>
        public Ship(JID owner, ShipType type, System.Windows.Controls.Orientation or, Field targetField)
        {
            this.Owner = owner;
            this.ShipType = type;
            this.IsPlaced = false;
            this.HitPoints = new Dictionary<int, bool>();
            this.Orientation = or;
            this.Position = targetField.Position;
            this.IsDestroyed = false;
            this.OverlayColor = Color.White;
            targetField.ReferencedShip = this;
            StartField = targetField;
            LoadTexture();

            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

        }


        /// <summary>
        /// Creates a new instance of a ship, which is not placed on the playground
        /// </summary>
        /// <param name="owner">The Jabber-ID of the owner</param>
        /// <param name="type">The type</param>                
        public Ship(JID owner, ShipType type, Boolean alternate)
        {
            this.Owner = owner;
            this.IsPlaced = false;
            this.ShipType = type;
            this.HitPoints = new Dictionary<int, bool>();
            this.OverlayColor = Color.White;
            this.IsDestroyed = false;

            switch (type)
            {
                case ShipType.DESTROYER:
                case ShipType.SUBMARINE:
                    this.Orientation = System.Windows.Controls.Orientation.Vertical;
                    break;
                case ShipType.BATTLESHIP:
                case ShipType.AIRCRAFT_CARRIER:
                    this.Orientation = System.Windows.Controls.Orientation.Horizontal;
                    break;
            }
            LoadTexture();

            // Ship is not placed. We place it manually
            switch (type)
            {
                case ShipType.DESTROYER:
                    this.Position = new Vector2(20, DeviceCache.BelowGrid + 20);
                    break;
                case ShipType.SUBMARINE:
                    if (alternate)
                        this.Position = new Vector2(20 + (1 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid + 20);
                    else
                        this.Position = new Vector2(20 + (5 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid + 20);
                    break;
                case ShipType.BATTLESHIP:
                    this.Position = new Vector2(20 + (2 * (this.shipTexture.Height * scaleRate)) + 20, DeviceCache.BelowGrid + 20);
                    break;
                case ShipType.AIRCRAFT_CARRIER:
                    this.Position = new Vector2(20 + (2 * (this.shipTexture.Height * scaleRate)) + 20, DeviceCache.BelowGrid + (this.shipTexture.Height * scaleRate) + 30);
                    break;
            }


            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

            
        }

        /// <summary>
        /// Updates the position of the ship
        /// </summary>
        public void UpdatePosition()
        {
            if (this.StartField != null)
            {
                this.Position = StartField.Position;
            }
        }

        /// <summary>
        /// Hits the ship on a specific field.
        /// If the field is found in the Fields-List, the specific HitPoint will be changed.
        /// </summary>
        /// <param name="f"></param>
        public void HitOnField(Field f)
        {
            for (int i = 0; i < this.Fields.Count; i++)
            {
                if (this.Fields[i].Equals(f))
                {
                    this.HitPoints[i] = true;
                }
            }
            CheckState();
        }

        /// <summary>
        /// Checks if all hitpoints are destroyed and updates the isDestroyed-Property of the ship
        /// </summary>
        private void CheckState()
        {
            Boolean destroyed = true;
            for (int i = 0; i < this.HitPoints.Count; i++)
            {
                if (!this.HitPoints[i])
                {
                    this.IsDestroyed = false;
                    return;
                }
            }
            this.IsDestroyed = destroyed;
        }

        /// <summary>
        /// Loads the texture of this ship by checking the ship type
        /// </summary>
        private void LoadTexture()
        {
            switch (this.ShipType)
            {
                case ShipType.DESTROYER:
                    this.Size = 2;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipDestroyerV;
                    else
                        this.shipTexture = TextureManager.ShipDestroyerH;
                    break;
                case ShipType.SUBMARINE:
                    this.Size = 3;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipSubmarineV;
                    else
                        this.shipTexture = TextureManager.ShipSubmarineH;
                    break;
                case ShipType.BATTLESHIP:
                    this.Size = 4;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipBattleshipV;
                    else
                        this.shipTexture = TextureManager.ShipBattleshipH;
                    break;
                case ShipType.AIRCRAFT_CARRIER:
                    this.Size = 5;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipAircraftcarrierV;
                    else
                        this.shipTexture = TextureManager.ShipAircraftcarrierH;
                    break;
            }
            this.scaleRate = calculateSizeRatio();
        }

        /// <summary>
        /// Sets the position of the ship
        /// </summary>
        /// <param name="p">The new position</param>
        public void SetPosition(Vector2 p)
        {
            this.Position = p;
        }

        /// <summary>
        /// Calculates the size ratio of the ship by taking the field with of the own playground
        /// </summary>
        /// <returns></returns>
        private float calculateSizeRatio()
        {
            Playground pg = null;
            if (this.Owner == AppCache.CurrentMatch.OwnJID)
            {
                pg = AppCache.CurrentMatch.OwnPlayground;
            }
            else
            {
                pg = AppCache.CurrentMatch.ShootingPlayground;
            }

            if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                return (float)pg.FieldSize.Width / this.shipTexture.Width;
            else
                return (float)pg.FieldSize.Width / this.shipTexture.Height;
        }

        /// <summary>
        /// Glues the ship to its referenced fields
        /// </summary>
        public void GlueToFields()
        {
            if (this.Fields != null)
            {
                foreach (Field f in Fields)
                {
                    f.ReferencedShip = null;
                }
            }
            List<Field> fields = CollissionManager.GetFields(AppCache.CurrentMatch.OwnPlayground, this);
            if (fields != null)
            {
                this.StartField = fields[0];
                this.Position = fields[0].Position;
                this.Fields = fields;
                foreach (Field f in fields)
                {
                    f.ReferencedShip = this;
                }
            }
        }

        /// <summary>
        /// Finishes the placement, so that the ship cannot moved again
        /// </summary>
        public void FinishPlacement()
        {
            if (this.Fields != null)
            {
                this.OverlayColor = Color.White;
                this.IsPlaced = true;
                AppCache.ActivePlacementShip = null;
                foreach (Field f in this.Fields)
                {
                    f.ResetColor();
                }
            }
            else
            {
                AppCache.CurrentMatch.OwnPlayground.Refresh();
            }
        }

        /// <summary>
        /// Toggles the orientation of the ship
        /// </summary>
        public void ToggleOrientation()
        {
            if (this.Orientation == System.Windows.Controls.Orientation.Horizontal)
            {
                this.Orientation = System.Windows.Controls.Orientation.Vertical;
            }
            else
            {
                this.Orientation = System.Windows.Controls.Orientation.Horizontal;
            }
            LoadTexture();
            GlueToFields();
            AppCache.CurrentMatch.OwnPlayground.Refresh();
        }

        /// <summary>
        /// Starts the movement of this ship by removing all referenced fields
        /// </summary>
        public void StartMovement()
        {
            this.StartField = null;
            if (this.Fields != null)
            {
                foreach (Field f in this.Fields)
                {
                    f.ReferencedShip = null;
                }
                this.Fields = null;
            }
        }

        /// <summary>
        /// Draws the ship to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shipTexture, new Vector2(Position.X, Position.Y), null, OverlayColor, 0f, Vector2.Zero, calculateSizeRatio(), SpriteEffects.None, 0f);            
        }
    }
}
