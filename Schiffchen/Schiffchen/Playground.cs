using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Schiffchen.GameElemens;
using Schiffchen.Logic;

namespace Schiffchen
{
    public class Playground
    {
        public static int PLAYGROUND_SIZE = 10;

        public Field[,] fields;

        public Playground()
        {
            double GridWidth = DeviceCache.ScreenWidth - (DeviceCache.ScreenWidth * 0.1);
            double GridHeight = DeviceCache.ScreenHeight - (DeviceCache.ScreenHeight * 0.4);
            double GridLeft = DeviceCache.ScreenWidth * 0.05;
            double GridTop = DeviceCache.ScreenHeight * 0.2;
            double FieldWidth = GridWidth / PLAYGROUND_SIZE;
           // double FieldHeight = GridHeight / PLAYGROUND_SIZE;
            double FieldHeight = FieldWidth;
            DeviceCache.FieldSize = new Size(FieldWidth, FieldHeight);

            fields = new Field[PLAYGROUND_SIZE, PLAYGROUND_SIZE];
            for (int r = 0; r < PLAYGROUND_SIZE; r++)
            {
                for (int c = 0; c < PLAYGROUND_SIZE; c++)
                {
                    Vector2 pos = new Vector2((float)(GridLeft + (c * FieldWidth)), (float)(GridTop + (r * FieldHeight)));
                    fields[r, c] = new Field(pos, new System.Windows.Size(FieldWidth, FieldHeight));

                    if (r == PLAYGROUND_SIZE - 1 && c == 0)
                    {
                        DeviceCache.BelowGrid = Convert.ToInt32(pos.Y + FieldHeight + 20);
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int r = 0; r < PLAYGROUND_SIZE; r++)
            {
                for (int c = 0; c < PLAYGROUND_SIZE; c++)
                {
                    fields[r, c].Draw(spriteBatch);
                }
            }
        }

    }
}
