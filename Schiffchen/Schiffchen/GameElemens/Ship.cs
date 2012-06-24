using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net.XMPP;
using Schiffchen.Resources;
using Schiffchen.Logic;

namespace Schiffchen.GameElemens
{
    public class Ship
    {
        public ShipType ShipType { get; private set; }
        public Vector2 Position { get; set; }
        public JID Owner { get; private set; }
        public Dictionary<int, bool> HitPoints { get; set; }
        public Boolean IsDestroyed { get; private set; }
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

        public void UpdatePosition()
        {
            if (this.StartField != null)
            {
                this.Position = StartField.Position;
            }
        }

        public Ship(JID owner, ShipType type, System.Windows.Controls.Orientation or, Field targetField)
        {
            this.Owner = owner;
            this.ShipType = type;
            this.IsPlaced = false;
            this.HitPoints = new Dictionary<int, bool>();
            this.Orientation = or;
            this.Position = targetField.Position;
            this.OverlayColor = Color.White;
            targetField.ReferencedShip = this;
            StartField = targetField;
            LoadTexture();

            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

        }

        public Ship(JID owner, ShipType type)
        {
            this.Owner = owner;
            this.IsPlaced = false;
            this.ShipType = type;
            this.HitPoints = new Dictionary<int, bool>();
            this.OverlayColor = Color.White;

            switch (type)
            {
                case Schiffchen.ShipType.DESTROYER:
                case Schiffchen.ShipType.SUBMARINE:
                    this.Orientation = System.Windows.Controls.Orientation.Vertical;
                    break;
                case Schiffchen.ShipType.BATTLESHIP:
                case Schiffchen.ShipType.AIRCRAFT_CARRIER:
                    this.Orientation = System.Windows.Controls.Orientation.Horizontal;
                    break;
            }
            LoadTexture();

            // Ship is not placed. We place it manually
            switch (type)
            {
                case Schiffchen.ShipType.DESTROYER:
                    this.Position = new Vector2(20, DeviceCache.BelowGrid + 20);
                    break;
                case Schiffchen.ShipType.SUBMARINE:
                    this.Position = new Vector2(20 + (1 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid + 20);
                    break;
                case Schiffchen.ShipType.BATTLESHIP:
                    this.Position = new Vector2(20 + (2 * (this.shipTexture.Height * scaleRate)) + 20, DeviceCache.BelowGrid + 20);
                    break;
                case Schiffchen.ShipType.AIRCRAFT_CARRIER:
                    this.Position = new Vector2(20 + (2 * (this.shipTexture.Height * scaleRate)) + 20, DeviceCache.BelowGrid + (this.shipTexture.Height * scaleRate) + 30);
                    break;
            }


            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

            
        }

        private void LoadTexture()
        {
            switch (this.ShipType)
            {
                case Schiffchen.ShipType.DESTROYER:
                    this.Size = 2;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipDestroyerV;
                    else
                        this.shipTexture = TextureManager.ShipDestroyerH;
                    break;
                case Schiffchen.ShipType.SUBMARINE:
                    this.Size = 3;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipSubmarineV;
                    else
                        this.shipTexture = TextureManager.ShipSubmarineH;
                    break;
                case Schiffchen.ShipType.BATTLESHIP:
                    this.Size = 4;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipBattleshipV;
                    else
                        this.shipTexture = TextureManager.ShipBattleshipH;
                    break;
                case Schiffchen.ShipType.AIRCRAFT_CARRIER:
                    this.Size = 5;
                    if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                        this.shipTexture = TextureManager.ShipAircraftcarrierV;
                    else
                        this.shipTexture = TextureManager.ShipAircraftcarrierH;
                    break;
            }
            this.scaleRate = calculateSizeRatio();
        }

        public void SetPosition(Vector2 p)
        {
            this.Position = p;
        }

        private float calculateSizeRatio()
        {
            if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                return (float)AppCache.CurrentMatch.OwnPlayground.FieldSize.Width / this.shipTexture.Width;
            else
                return (float)AppCache.CurrentMatch.OwnPlayground.FieldSize.Width / this.shipTexture.Height;
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(shipTexture, new Vector2(Position.X, Position.Y), null, OverlayColor, 0f, Vector2.Zero, calculateSizeRatio(), SpriteEffects.None, 0f);
        }
    }
}
