using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Schiffchen.Resources;

namespace Schiffchen.GameElemens
{
    public class Field
    {
        public Vector2 Position { get; private set; }
        public Size Size { get; private set; }
        public Ship ReferencedShip { get; set; }

        public Field(Vector2 p, Size s)
        {
            this.Position = p;
            this.Size = s;
        }

        public Field()
        {

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), 2, Convert.ToInt32(Size.Height)), Color.White); // Left
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X+Size.Width), Convert.ToInt32(Position.Y), 2, Convert.ToInt32(Size.Height)), Color.White); // Right
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.Width), 2), Color.White); // Top
            spriteBatch.Draw(TextureManager.White, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y + Size.Height), Convert.ToInt32(Size.Width), 2), Color.White); // Bottom
        }
    }
}
