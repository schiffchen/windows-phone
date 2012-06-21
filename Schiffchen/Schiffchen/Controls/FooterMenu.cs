using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Schiffchen.Logic;
using Schiffchen.Resources;

namespace Schiffchen.Controls
{
    

    public class FooterMenu
    {
        public Rectangle Rectangle { get; private set; }
        public IconButton[] Buttons;

        private float alpha = 0.5f;
        private Color color;
   
        public FooterMenu(int TopLine, int Height)
        {
            this.Rectangle = new Rectangle(0, TopLine, DeviceCache.ScreenWidth, Height);
            this.alpha = 0.5f;
            this.color = new Color(255,255,255) * alpha;
            Buttons = new IconButton[2];
        }

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

        public void RemoveButton(String id)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].ID.Equals(id))
                {
                    Buttons[i].DoRemove();
                    Buttons[i] = null;
                }
            }
        }

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

        public void Update()
        {
        }

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
        }

        public void RemoveAllButtons()
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = null;
            }
        }

        

    }
}
