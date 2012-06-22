using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Text;

namespace Schiffchen.Resources
{
    public static class FontManager
    {

        public static SpriteFont MenuFont;
        public static SpriteFont GameFont;
        public static SpriteFont InfoFont;
        public static SpriteFont ButtonFont;
        public static SpriteFont DiceFont;



        public static void LoadContent(ContentManager Content)
        {
            MenuFont = Content.Load<SpriteFont>("font\\menuFont");
            GameFont = Content.Load<SpriteFont>("font\\gameFont");
            InfoFont = Content.Load<SpriteFont>("font\\infoFont");
            ButtonFont = Content.Load<SpriteFont>("font\\buttonFont");
            DiceFont = Content.Load<SpriteFont>("font\\diceFont");
        }
    }
}
