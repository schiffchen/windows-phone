using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Schiffchen.Resources
{
    public static class TextureManager
    {
        public static Texture2D GameBackground;
        public static Texture2D Transparent;
        public static Texture2D Black;
        public static Texture2D White;
        public static Texture2D Red;
        public static Texture2D Green;
        public static Texture2D ShipAircraftcarrierV;
        public static Texture2D ShipBattleshipV;
        public static Texture2D ShipDestroyerV;
        public static Texture2D ShipSubmarineV;
        public static Texture2D ShipAircraftcarrierH;
        public static Texture2D ShipBattleshipH;
        public static Texture2D ShipDestroyerH;
        public static Texture2D ShipSubmarineH;

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
            Black = new Texture2D(device, 1, 1);
            Black.SetData(new[] { Color.Black });
            White = new Texture2D(device, 1, 1);
            White.SetData(new[] { Color.White });
            Red = new Texture2D(device, 1, 1);
            Red.SetData(new[] { Color.Red });
            Green = new Texture2D(device, 1, 1);
            Green.SetData(new[] { Color.Green });
        }
    }
}
