using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Windows;
using Schiffchen.GameElemens;
using Microsoft.Xna.Framework.Input.Touch;
using Schiffchen.Logic;
using Schiffchen.Logic.Enum;
using Schiffchen.Event;

namespace Schiffchen.GameElemens
{
    public class Playground
    {
        public static int PLAYGROUND_SIZE = 10;
        public Size FieldSize;
        public event EventHandler<EventArgs> Click;
        public event EventHandler<ShootEventArgs> TargetSelected;
        public Rectangle Rectangle { get; private set; }

        public PlaygroundMode PlaygroundMode { get; private set; }
        public PlaygroundType PlaygroundType { get; private set; }

        public Field[,] fields;

        #region Sizes
        private double GridWidth;
        private double GridLeft;
        private double GridHeight;
        private double FieldWidth;
        private double FieldHeight;
        private double GridTop;
        private float SizeRatio;
        private Boolean isIncreaseToMain;
        private Boolean isReduceToMinimap;
        #endregion


        public Playground(PlaygroundMode mode)
        {
            this.PlaygroundMode = mode;
            if (mode == Logic.Enum.PlaygroundMode.Minimap)
            {
                this.PlaygroundType = Logic.Enum.PlaygroundType.ShootingView;
                SizeRatio = 0.3f;
            }
            else if (mode == Logic.Enum.PlaygroundMode.Normal)
            {
                this.PlaygroundType = Logic.Enum.PlaygroundType.ShipView;
                SizeRatio = 1f;
            }
            
            CalculateSizes();
            CalculateFields();
        }

        public void Refresh()
        {
            foreach (Ship s in AppCache.CurrentMatch.OwnShips)
            {
                s.GlueToFields();
            }
            foreach (Field f in this.fields)
            {
                if (f.ReferencedShip == null)
                {
                    f.ResetColor();
                }                
            }
        }

        private void Increase()
        {
            this.SizeRatio = MathHelper.Clamp(this.SizeRatio + 0.1f, 0.3f, 1f);
            this.CalculateSizes();
            this.CalculateFields();
        }

        private void Reduce()
        {
            this.SizeRatio = MathHelper.Clamp(this.SizeRatio - 0.1f, 0.3f, 1f);
            this.CalculateSizes();
            this.CalculateFields();
        }

        public void ReduceToMinimap()
        {
            this.isReduceToMinimap = true;
        }

        public void IncreaseToMain()
        {
            this.isIncreaseToMain = true;
        }

        public void ResetFieldColors()
        {
            foreach (Field f in this.fields)
            {
                f.ResetColor();
            }
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
                        fields[r, c] = new Field(pos, new System.Windows.Size(FieldWidth, FieldHeight), c + 1, r + 1);
                    }
                    else
                    {
                        fields[r, c].SetProperties(pos, new System.Windows.Size(FieldWidth, FieldHeight));
                        if (fields[r, c].ReferencedShip != null)
                        {
                            fields[r, c].ReferencedShip.UpdatePosition();
                        }
                    }

                    if (firstStarted)
                    {
                        if (r == 0 && c == PLAYGROUND_SIZE - 1)
                        {
                            DeviceCache.RightOfMinimap = new Vector2((float)(pos.X + FieldWidth + 20), pos.Y);
                        }
                        else if (r == PLAYGROUND_SIZE - 1 && c == 0)
                        {
                            if (this.PlaygroundMode == Logic.Enum.PlaygroundMode.Normal)
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
            if (this.PlaygroundMode == Logic.Enum.PlaygroundMode.Minimap)
            {
                int x = Convert.ToInt32(gs.Position.X);
                int y = Convert.ToInt32(gs.Position.Y);
                if (this.Rectangle.Contains(x, y))
                {
                    this.OnClick(new EventArgs());
                }
            }
            if (AppCache.CurrentMatch.MatchState == MatchState.Playing && AppCache.CurrentMatch.IsMyTurn)
            {
                if (this.PlaygroundMode == PlaygroundMode.Normal)
                {
                    foreach (Field f in this.fields)
                    {
                        if (f.IsClicked(gs))
                        {
                            OnTargetSelected(new ShootEventArgs(f.X, f.Y));
                        }
                    }
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

        public void Update()
        {
            if (isIncreaseToMain)
            {
                this.isReduceToMinimap = false;
                this.Increase();
                if (this.SizeRatio == 1f)
                {
                    this.isIncreaseToMain = false;
                    this.PlaygroundMode = Logic.Enum.PlaygroundMode.Normal;
                }
            }
            if (isReduceToMinimap)
            {
                this.isIncreaseToMain = false;
                this.Reduce();
                if (this.SizeRatio == 0.3f)
                {
                    this.isReduceToMinimap = false;
                    this.PlaygroundMode = Logic.Enum.PlaygroundMode.Minimap;
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

        protected virtual void OnTargetSelected(ShootEventArgs e)
        {
            EventHandler<ShootEventArgs> handler = TargetSelected;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

    }
}
