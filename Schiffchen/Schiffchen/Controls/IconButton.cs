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
    /// <summary>
    /// Represents a custom button, which can render a texture
    /// </summary>
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

        /// <summary>
        /// Creates a new instance of IconButton
        /// </summary>
        /// <param name="icon">The icon texture</param>
        /// <param name="position">The target position</param>
        /// <param name="scaleRate">The scale rate</param>
        /// <param name="text">The button caption</param>
        /// <param name="id">The ID of the button</param>
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

        /// <summary>
        /// Creates a new instance of IconButton.
        /// Constructor is used for buttons, which are added to the FooterMenu
        /// </summary>
        /// <param name="icon">The icon texture</param>
        /// <param name="text">The button caption</param>
        /// <param name="id">The ID of the button</param>
        public IconButton(Texture2D icon, String text, String id)
        {
            this.Icon = icon;
            this.ScaleRate = 2f;
            this.Visible = true;
            this.Text = text;
            this.ID = id;
        }       

        /// <summary>
        /// Sets the position of a button
        /// </summary>
        /// <param name="pos">The new position</param>
        public void SetPosition(Vector2 pos)
        {
            this.Position = pos;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Icon.Width * ScaleRate), Convert.ToInt32(Icon.Height * ScaleRate));
        }              

        /// <summary>
        /// Checks if the button is clicked.
        /// When the button is clicked, the OnClick-Event is called
        /// </summary>
        /// <param name="gs"></param>
        public void CheckClick(GestureSample gs)
        {
            int x = Convert.ToInt32(gs.Position.X);
            int y = Convert.ToInt32(gs.Position.Y);
            if (this.Rectangle.Contains(x, y))
            {
                this.OnClick(new EventArgs());
            }
        }
        
        /// <summary>
        /// Hides the button and calls the OnRemove-Event
        /// </summary>
        public void DoRemove()
        {
            this.Visible = false;
            this.OnRemove(new EventArgs());
        }

        /// <summary>
        /// Fires when the button is clicked
        /// </summary>
        /// <param name="e">The event arguments</param>
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

        /// <summary>
        /// Fires when the button is removed
        /// </summary>
        /// <param name="e">The event arguments</param>
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


        /// <summary>
        /// Draws the button to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible)
            {
                Vector2 textSize = FontManager.ButtonFont.MeasureString(this.Text);
                Vector2 textCenter = new Vector2((this.Rectangle.Left + this.Rectangle.Width / 2) - textSize.X / 2, this.Position.Y + (this.Icon.Height * ScaleRate) + 5);
                spriteBatch.Draw(Icon, Position, null, Color.White, 0f, Vector2.Zero, this.ScaleRate, SpriteEffects.None, 0);
                spriteBatch.DrawString(FontManager.ButtonFont, this.Text, textCenter, Color.White);
            }
        }

    }
}
