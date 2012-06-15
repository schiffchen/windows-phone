using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net.XMPP;
using Schiffchen.Resources;

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

        private Texture2D shipTexture;
        private float scaleRate;

        public Ship(JID owner, ShipType type, System.Windows.Controls.Orientation or, Field targetField)
        {
            this.Owner = owner;
            this.ShipType = type;
            this.HitPoints = new Dictionary<int, bool>();
            this.Orientation = or;
            this.Position = targetField.Position;
            targetField.ReferencedShip = this;
            StartField = targetField;
            LoadTexture();

            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

            this.scaleRate = calculateSizeRatio();
        }

        public Ship(JID owner, ShipType type)
        {
            this.Owner = owner;
            this.ShipType = type;
            this.HitPoints = new Dictionary<int, bool>();
            this.Orientation = System.Windows.Controls.Orientation.Vertical;
            

            LoadTexture();

            

            for (int i = 0; i < this.Size; i++)
            {
                this.HitPoints.Add(i, false);
            }

            this.scaleRate = calculateSizeRatio();

            // Ship is not placed. We place it manually
            switch (type)
            {
                case Schiffchen.ShipType.DESTROYER:
                    this.Position = new Vector2(20, DeviceCache.BelowGrid);
                    break;
                case Schiffchen.ShipType.SUBMARINE:
                    this.Position = new Vector2(20 + (1 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid);
                    break;
                case Schiffchen.ShipType.BATTLESHIP:
                    this.Position = new Vector2(20 + (2 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid);
                    break;
                case Schiffchen.ShipType.AIRCRAFT_CARRIER:
                    this.Position = new Vector2(20 + (3 * (this.shipTexture.Width * scaleRate)) + 20, DeviceCache.BelowGrid);
                    break;
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
            calculateSizeRatio();
        }

        public void SetPosition(Vector2 p)
        {
            this.Position = p;
        }

        private float calculateSizeRatio()
        {
            if (this.Orientation == System.Windows.Controls.Orientation.Vertical)
                return (float)DeviceCache.FieldSize.Width / this.shipTexture.Width;
            else
                return (float)DeviceCache.FieldSize.Width / this.shipTexture.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(shipTexture, new Vector2(Position.X, Position.Y), null, Color.White, 0f, Vector2.Zero, calculateSizeRatio(), SpriteEffects.None, 0f);
        }
    }
}
