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

        public Field(Vector2 p, Size s, int x, int y)
        {
            this.Position = p;
            this.Size = s;
            this.backgroundColor = null;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height));
            this.X = x;
            this.Y = y;
        }

        public void SetProperties(Vector2 pos, Size size)
        {
            this.Position = pos;
            this.Size = size;
            this.Rectangle = new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height));
        }

        public Field()
        {
            this.backgroundColor = null;
        }

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

        public void ResetColor()
        {
            this.backgroundColor = null;
        }


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
        }

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
    }
}
