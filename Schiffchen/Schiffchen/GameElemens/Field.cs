using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Schiffchen.Resources;
using Schiffchen.Logic.Enum;
using Microsoft.Xna.Framework.Input.Touch;

namespace Schiffchen.GameElemens
{
    /// <summary>
    /// Represents a single field of the playground
    /// </summary>
    public class Field
    {
        public Vector2 Position { get; private set; }
        public Size Size { get; private set; }
        public Ship ReferencedShip { get; set; }
        public FieldState FieldState { get; set; }
        private Texture2D backgroundColor;
        public Int32 X { get; private set; }
        public Int32 Y { get; private set; }

        public Rectangle Rectangle { get; private set; }

        /// <summary>
        /// Creates a new instance of a field
        /// </summary>
        /// <param name="p">The target position</param>
        /// <param name="s">The size</param>
        /// <param name="x">The X-Coordinate</param>
        /// <param name="y">The Y-Coordinate</param>
        public Field(Vector2 p, Size s, int x, int y)
        {
            this.Position = p;
            this.Size = s;
            this.backgroundColor = null;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height));
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Sets the Position and Size of the field
        /// </summary>
        /// <param name="pos">The new position</param>
        /// <param name="size">The new size</param>
        public void SetProperties(Vector2 pos, Size size)
        {
            this.Position = pos;
            this.Size = size;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height));
        }        

        /// <summary>
        /// Sets the background color of a field
        /// </summary>
        /// <param name="color">The given FieldColor</param>
        public void SetColor(FieldColor color)
        {
            switch (color)
            {
                case FieldColor.Black:
                    this.backgroundColor = TextureManager.Black;
                    break;
                case FieldColor.Green:
                    this.backgroundColor = TextureManager.Green;
                    break;
                case FieldColor.Red:
                    this.backgroundColor = TextureManager.Red;
                    break;
                case FieldColor.White:
                    this.backgroundColor = TextureManager.White;
                    break;
            }
        }

        /// <summary>
        /// Resets the background color to transparent
        /// </summary>
        public void ResetColor()
        {
            this.backgroundColor = null;
        }

        /// <summary>
        /// Checks if the field is clicked
        /// </summary>
        /// <param name="gs">The given GestureSample</param>
        /// <returns>True if clicked, false if not.</returns>
        public Boolean IsClicked(GestureSample gs)
        {
            int x = Convert.ToInt32(gs.Position.X);
            int y = Convert.ToInt32(gs.Position.Y);
            if (this.Rectangle.Contains(x, y))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draws the field to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.backgroundColor != null)
            {
                spriteBatch.Draw(backgroundColor, new Rectangle(Rectangle.X + 2, Rectangle.Y + 2, Rectangle.Width - 2, Rectangle.Height - 2), Color.White);
            }

            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), 2, Convert.ToInt32(Size.Height)), Color.White); // Left
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X + Size.Width), Convert.ToInt32(Position.Y), 2, Convert.ToInt32(Size.Height)), Color.White); // Right
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), 2), Color.White); // Top
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y + Size.Height), Convert.ToInt32(Size.Width), 2), Color.White); // Bottom

            
            
            switch (FieldState) {
                case Logic.Enum.FieldState.Hit:
                    float symratio1 = (float)((this.Size.Width - 4) / TextureManager.FieldHit.Width);
                    spriteBatch.Draw(TextureManager.FieldHit, new Vector2(Rectangle.X + 2, Rectangle.Y + 2), null, Color.White, 0f, Vector2.Zero, symratio1, SpriteEffects.None, 1f);
                    break;
                case Logic.Enum.FieldState.Water:
                    float symratio2 = (float)((this.Size.Width - 4) / TextureManager.FieldWater.Width);
                    spriteBatch.Draw(TextureManager.FieldWater, new Vector2(Rectangle.X + 2, Rectangle.Y + 2), null, Color.White, 0f, Vector2.Zero, symratio2, SpriteEffects.None, 1f);
                    break;
            }
        }
    }
}
