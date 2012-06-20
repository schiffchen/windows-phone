using System;
using System.Net;
using System.Windows.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Schiffchen.Logic;

namespace Schiffchen.Controls
{
    public class IconButton
    {
        public Texture2D Icon { get; set; }
        public Vector2 Position { get; set; }
        public float ScaleRate { get; set; }
        public Boolean Visible {get; set; }
        public Rectangle Rectangle { get; private set; }
        public event EventHandler<EventArgs> Click;

        public IconButton(Texture2D icon, Vector2 position, float scaleRate)
        {
            this.Icon = icon;
            this.Position = position;
            this.ScaleRate = scaleRate;
            this.Visible = true;
            this.Rectangle = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(icon.Width * ScaleRate), Convert.ToInt32(icon.Height * ScaleRate));
        }

        public IconButton(Texture2D icon, Vector2 position)
        {
            this.Icon = icon;
            this.Position = position;
            this.ScaleRate = 2f;
            this.Visible = true;
            this.Rectangle = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), icon.Width, icon.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                spriteBatch.Draw(Icon, Position, null, Color.White, 0f, Vector2.Zero, this.ScaleRate, SpriteEffects.None, 0);
            }
        }

        public void CheckClick(GestureSample gs)
        {
            int x = Convert.ToInt32(gs.Position.X);
            int y = Convert.ToInt32(gs.Position.Y);
            if (this.Rectangle.Contains(x, y))
            {
                this.OnClick(new EventArgs());
            }
        }

        protected virtual void OnClick(EventArgs e)
        {          
            EventHandler<EventArgs> handler = Click;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }


    }
}
