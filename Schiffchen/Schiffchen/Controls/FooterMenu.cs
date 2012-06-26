using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Schiffchen.Logic;
using Schiffchen.Resources;
using Schiffchen.GameElemens;

namespace Schiffchen.Controls
{
    
    /// <summary>
    /// Defines a menu, which can hold dices and buttons
    /// </summary>
    public class FooterMenu
    {
        public Rectangle Rectangle { get; private set; }
        public IconButton[] Buttons;
        public Dice[] Dices;

        private float alpha = 0.5f;
        private Color color;
   
        /// <summary>
        /// Creates a new instance of the FooterMenu
        /// </summary>
        /// <param name="TopLine">The top line of the menu. Should be below other content</param>
        /// <param name="Height">The height of the menu.</param>
        public FooterMenu(int TopLine, int Height)
        {
            this.Rectangle = new Rectangle(0, TopLine, DeviceCache.ScreenWidth, Height);
            this.alpha = 0.5f;
            this.color = new Color(255,255,255) * alpha;
            Buttons = new IconButton[2];
            Dices = new Dice[2];
        }

        /// <summary>
        /// Adds an IconButton to the menu.
        /// If no free slot is available, an error will be thrown.
        /// </summary>
        /// <param name="button">The button which should be added</param>
        public void AddButton(IconButton button)
        {
            if (Buttons[1] == null)
            {
                button.SetPosition(new Vector2(this.Rectangle.Width - 70, this.Rectangle.Top + 10));
                Buttons[1] = button;
            }
            else if (Buttons[0] == null)
            {
                button.SetPosition(new Vector2(this.Rectangle.Width - 140, this.Rectangle.Top + 10));
                Buttons[0] = button;
            }
            else
            {
                throw new Exception("No free slot available!");
            }
        }

        /// <summary>
        /// Removes the given button from the menu
        /// </summary>
        /// <param name="button">The button to remove</param>
        public void RemoveButton(IconButton button)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].Equals(button))
                {
                    Buttons[i].DoRemove();
                    Buttons[i] = null;
                }
            }
        }

        /// <summary>
        /// Removes the button with the given ID from the menu
        /// </summary>
        /// <param name="id">The ID of the button to remove</param>
        public void RemoveButton(String id)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {                
                if (Buttons[i] != null && Buttons[i].ID.Equals(id))
                {
                    Buttons[i].DoRemove();
                    Buttons[i] = null;
                }
            }
        }

        /// <summary>
        /// Returns the button with the given ID
        /// </summary>
        /// <param name="ID">The ID of the button to search for</param>
        /// <returns>The button with the given ID</returns>
        public IconButton Get(String ID)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].ID.Equals(ID))
                {
                    return Buttons[i];
                }
            }
            return null;
        }


        /// <summary>
        /// Removes all buttons from the menu
        /// </summary>
        public void RemoveAllButtons()
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = null;
            }
        }
      
        /// <summary>
        /// Draws the Menu and its buttons and dices to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Black, Rectangle, color);
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i] != null)
                {
                    Buttons[i].Draw(spriteBatch);
                }
            }
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Dices[i] != null)
                {
                    Dices[i].Draw(spriteBatch);
                }
            }
        }        
    }
}
