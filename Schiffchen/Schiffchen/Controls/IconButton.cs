using System;
using System.Net;
using System.Windows.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Schiffchen.Logic;
using Schiffchen.Resources;

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
        public event EventHandler<EventArgs> Remove;
        public String Text { get; set; }
        public String ID { get; set; }

        public IconButton(Texture2D icon, Vector2 position, float scaleRate, String text, String id)
        {
            this.Icon = icon;
            this.Position = position;
            this.ScaleRate = scaleRate;
            this.Visible = true;
            this.Rectangle = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(icon.Width * ScaleRate), Convert.ToInt32(icon.Height * ScaleRate));
            this.Text = text;
            this.ID = id;
        }

        public IconButton(Texture2D icon, String text, String id)
        {
            this.Icon = icon;
            this.ScaleRate = 2f;
            this.Visible = true;
            this.Text = text;
            this.ID = id;
        }

        public void SetPosition(Vector2 pos)
        {
            this.Position = pos;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Icon.Width * ScaleRate), Convert.ToInt32(Icon.Height * ScaleRate));
        }

        public IconButton(Texture2D icon, Vector2 position, String text, String id)
        {
            this.Icon = icon;
            this.Position = position;
            this.ScaleRate = 2f;
            this.Visible = true;
            this.Rectangle = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), Convert.ToInt32(icon.Width * ScaleRate), Convert.ToInt32(icon.Height * ScaleRate) + 10);
            this.Text = text;
            this.ID = id;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                Vector2 textSize = FontManager.ButtonFont.MeasureString(this.Text);
                Vector2 textCenter = new Vector2((this.Rectangle.Left + this.Rectangle.Width / 2) - textSize.X / 2 , this.Position.Y + (this.Icon.Height * ScaleRate) + 5);
                spriteBatch.Draw(Icon, Position, null, Color.White, 0f, Vector2.Zero, this.ScaleRate, SpriteEffects.None, 0);
                spriteBatch.DrawString(FontManager.ButtonFont, this.Text, textCenter, Color.White);
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

        

        public void DoRemove()
        {
            this.Visible = false;
            this.OnRemove(new EventArgs());
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

        protected virtual void OnRemove(EventArgs e)
        {
            EventHandler<EventArgs> handler = Remove;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }


    }
}
