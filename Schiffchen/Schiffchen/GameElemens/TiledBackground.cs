using System;
using System.Net;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Schiffchen.GameElemens
{
    /// <summary>
    /// This is the Background
    /// </summary>
    public class TiledBackground
    {
        private readonly Texture2D _texture;
        readonly int _horizontalTileCount;
        readonly int _verticalTileCount;
        public Vector2 _startCoord;

        /// <summary>
        /// Creates a new tiled background
        /// </summary>
        /// <param name="texture">The texture</param>
        /// <param name="environmentWidth">The width of the screen</param>
        /// <param name="environmentHeight">The height of the screen</param>
        public TiledBackground(Texture2D texture, int environmentWidth, int environmentHeight)
        {
            _texture = texture;
            _horizontalTileCount = (int)(Math.Round((double)environmentWidth / _texture.Width) + 1);
            _verticalTileCount = (int)(Math.Round((double)environmentHeight / _texture.Height) + 1);

            _startCoord = new Vector2(0, 0);
        }

        /// <summary>
        /// Updates the background
        /// </summary>
        /// <param name="_cameraRectangle"></param>
        public void Update(Rectangle _cameraRectangle)
        {
            _startCoord.X = ((_cameraRectangle.X / _texture.Width) * _texture.Width) - _cameraRectangle.X;
            _startCoord.Y = ((_cameraRectangle.Y / _texture.Height) * _texture.Height) - _cameraRectangle.Y;
        }

        /// <summary>
        /// Draws the background to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _horizontalTileCount; i++)
            {
                for (int j = 0; j < _verticalTileCount; j++)
                {
                    spriteBatch.Draw(_texture,
                    new Rectangle(
                    (int)_startCoord.X + (i * _texture.Width),
                    (int)_startCoord.Y + (j * _texture.Height),
                    _texture.Width, _texture.Height),
                    Color.White);
                }
            }
        }
    }
}
