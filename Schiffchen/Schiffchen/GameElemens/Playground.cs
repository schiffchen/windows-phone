using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows;
using Schiffchen.GameElemens;
using Microsoft.Xna.Framework.Input.Touch;
using Schiffchen.Logic;

namespace Schiffchen.GameElemens
{
    public class Playground
    {
        public static int PLAYGROUND_SIZE = 10;
        public Size FieldSize;
        public Boolean IsBig { get; private set; }
        public event EventHandler<EventArgs> Click;
        public Rectangle Rectangle { get; private set; }

        private Boolean isInResizeProcess;

        public Field[,] fields;

        #region Sizes
        private double GridWidth;
        private double GridLeft;
        private double GridHeight;
        private double FieldWidth;
        private double FieldHeight;
        private double GridTop;
        private float SizeRatio;
        #endregion


        public Playground(float sizeRatio)
        {
            if (sizeRatio == 1f)
            {
                this.IsBig = true;
            }
            else
            {
                this.IsBig = false;
            }
            this.SizeRatio = sizeRatio;
            CalculateSizes();
            CalculateFields();

        }

        public void Increase()
        {
            this.SizeRatio += 0.1f;
            this.CalculateSizes();
            this.CalculateFields();
        }

        public void Reduce()
        {
            this.SizeRatio -= 0.1f;
            this.CalculateSizes();
            this.CalculateFields();
        }

        public void CalculateFields()
        {
            bool firstStarted = false;
            if (fields == null)
            {
                firstStarted = true;
                fields = new Field[PLAYGROUND_SIZE, PLAYGROUND_SIZE];
            }

            for (int r = 0; r < PLAYGROUND_SIZE; r++)
            {
                for (int c = 0; c < PLAYGROUND_SIZE; c++)
                {
                    Vector2 pos = new Vector2((float)(GridLeft + (c * FieldWidth)), (float)(GridTop + (r * FieldHeight)));
                    if (fields[r, c] == null)
                    {
                        fields[r, c] = new Field(pos, new System.Windows.Size(FieldWidth, FieldHeight));
                    }
                    else
                    {
                        fields[r, c].SetProperties(pos, new System.Windows.Size(FieldWidth, FieldHeight));
                    }

                    if (firstStarted)
                    {
                        if (r == PLAYGROUND_SIZE - 1 && c == 0)
                        {
                            if (this.IsBig)
                            {
                                DeviceCache.BelowGrid = Convert.ToInt32(pos.Y + FieldHeight + 20);
                            }
                            else
                            {
                                DeviceCache.BelowSmallGrid = Convert.ToInt32(pos.Y + FieldHeight + 20);
                            }
                        }
                    }

                }
            }
        }

        public void CheckClick(GestureSample gs)
        {
            if (!this.IsBig)
            {
                int x = Convert.ToInt32(gs.Position.X);
                int y = Convert.ToInt32(gs.Position.Y);
                if (this.Rectangle.Contains(x, y))
                {
                    this.OnClick(new EventArgs());
                }
            }
        }

        public void CalculateSizes()
        {
            GridWidth = (DeviceCache.ScreenWidth - (DeviceCache.ScreenWidth * 0.1)) * this.SizeRatio;
            GridHeight = (DeviceCache.ScreenHeight - (DeviceCache.ScreenHeight * 0.4)) * this.SizeRatio;
            GridLeft = (DeviceCache.ScreenWidth * 0.05);


            FieldWidth = GridWidth / PLAYGROUND_SIZE;
            FieldHeight = FieldWidth;
   
            if (this.SizeRatio == 1f)
            {
                GridTop = (DeviceCache.ScreenHeight - (FieldHeight * PLAYGROUND_SIZE) - 200);
            }
            else
            {
                GridTop = 20;
            }
            this.FieldSize = new Size(FieldWidth, FieldHeight);
            this.Rectangle = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(GridLeft), Convert.ToInt32(GridTop), Convert.ToInt32(GridWidth), Convert.ToInt32(GridHeight));
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
