using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Schiffchen.Resources
{
    /// <summary>
    /// Handles all used textures in the XNA part of the game
    /// </summary>
    public static class TextureManager
    {
        public static Texture2D GameBackground;
        public static Texture2D Transparent;
        public static Texture2D Black;
        public static Texture2D White;
        public static Texture2D Red;
        public static Texture2D Green;
        public static Texture2D Gray;
        public static Texture2D Yellow;
        public static Texture2D DarkGray;
        public static Texture2D ShipAircraftcarrierV;
        public static Texture2D ShipBattleshipV;
        public static Texture2D ShipDestroyerV;
        public static Texture2D ShipSubmarineV;
        public static Texture2D ShipAircraftcarrierH;
        public static Texture2D ShipBattleshipH;
        public static Texture2D ShipDestroyerH;
        public static Texture2D ShipSubmarineH;
        public static Texture2D FieldHit;
        public static Texture2D FieldWater;

        public static Texture2D IconAccept;
        public static Texture2D IconTurn;
        public static Texture2D IconCancel;
        public static Texture2D IconAttack;
        public static Texture2D IconAttackSW;

        public static Texture2D SymbolOnline;
        public static Texture2D SymbolWaiting;
        public static Texture2D SymbolOffline;

        /// <summary>
        /// Loads all Content
        /// </summary>
        /// <param name="Content">The ContentManager</param>
        /// <param name="device">The GrahicsDevice</param>
        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDevice device)
        {
            GameBackground = Content.Load<Texture2D>("background\\water");
            ShipAircraftcarrierV = Content.Load<Texture2D>("ships\\aircraftcarrier400v");
            ShipBattleshipV = Content.Load<Texture2D>("ships\\battleship400v");
            ShipDestroyerV = Content.Load<Texture2D>("ships\\destroyer400v");
            ShipSubmarineV = Content.Load<Texture2D>("ships\\submarine400v");
            ShipAircraftcarrierH = Content.Load<Texture2D>("ships\\aircraftcarrier400h");
            ShipBattleshipH = Content.Load<Texture2D>("ships\\battleship400h");
            ShipDestroyerH = Content.Load<Texture2D>("ships\\destroyer400h");
            ShipSubmarineH = Content.Load<Texture2D>("ships\\submarine400h");
            Transparent = Content.Load<Texture2D>("misc\\transp");
            IconAccept = Content.Load<Texture2D>("icons\\accept");
            IconCancel = Content.Load<Texture2D>("icons\\cancel");
            IconTurn = Content.Load<Texture2D>("icons\\turn");
            IconAttack = Content.Load<Texture2D>("icons\\attack");
            IconAttackSW = Content.Load<Texture2D>("icons\\attackDisabled");
            SymbolOffline = Content.Load<Texture2D>("icons\\offline");
            SymbolWaiting = Content.Load<Texture2D>("icons\\waiting");
            SymbolOnline = Content.Load<Texture2D>("icons\\online");
           
            FieldHit = Content.Load<Texture2D>("icons\\hit");
            FieldWater = Content.Load<Texture2D>("icons\\water");
            Black = new Texture2D(device, 1, 1);
            Black.SetData(new[] { Color.Black });
            White = new Texture2D(device, 1, 1);
            White.SetData(new[] { Color.White });
            Red = new Texture2D(device, 1, 1);
            Red.SetData(new[] { Color.Red });
            Green = new Texture2D(device, 1, 1);
            Green.SetData(new[] { Color.Green });

            Gray = new Texture2D(device, 1, 1);
            Gray.SetData(new[] { Color.Gray });
            Yellow = new Texture2D(device, 1, 1);
            Yellow.SetData(new[] { Color.Yellow });
            DarkGray = new Texture2D(device, 1, 1);
            DarkGray.SetData(new[] { Color.DarkGray });
        }
    }
}
