using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Text;

namespace Schiffchen.Resources
{
    /// <summary>
    /// Handles all used fonts in the XNA part of the game
    /// </summary>
    public static class FontManager
    {

        public static SpriteFont MenuFont;
        public static SpriteFont GameFont;
        public static SpriteFont InfoFont;
        public static SpriteFont ButtonFont;
        public static SpriteFont DiceFont;


        /// <summary>
        /// Loads all content
        /// </summary>
        /// <param name="Content">The ContentManager</param>
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
