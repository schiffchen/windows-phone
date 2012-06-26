using System;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Schiffchen.Resources;
using Schiffchen.Logic;
using Schiffchen.Event;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Input.Touch;

namespace Schiffchen.GameElemens
{
    /// <summary>
    /// Represents a dice
    /// </summary>
    public class Dice
    {
        public String Value { get; private set; }
        public Rectangle Rectangle { get; private set; }
        public Boolean ReadOnly { get; set; }
        public event EventHandler<RollingDiceEventArgs> RollingFinish;
        public event EventHandler<EventArgs> BlinkComplete;
        public event EventHandler<EventArgs> Click;
        public String Caption { get; private set; }
        

        private int targetValue;
        private bool isRolling;
        private DispatcherTimer timer;
        private DispatcherTimer blinkTimer;
        private int timerCounter;
        private Random rnd;
        private Boolean blinkState;
        private int blinkCounter;
        private Texture2D blinkColor;

        /// <summary>
        /// Creates a new instance of a dice
        /// </summary>
        /// <param name="pos">The target position</param>
        /// <param name="caption">The caption text</param>
        public Dice(Vector2 pos, String caption)
        {
            this.Rectangle = new Rectangle(Convert.ToInt32(pos.X) + 20, DeviceCache.BelowGrid + Convert.ToInt32(pos.Y), 80, 80);
            timerCounter = 0;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += new EventHandler(timer_Tick);
            
            this.Caption = caption;
            this.ReadOnly = false;
            this.ResetValue();
            this.targetValue = -1;
            this.blinkState = false;
            this.blinkCounter = 0;
            
        }

        /// <summary>
        /// Is called when the timer ticks and is part of the rolling animation
        /// </summary>
        /// <param name="sender">The timer</param>
        /// <param name="e">The event arguments</param>
        void timer_Tick(object sender, EventArgs e)
        {
            timerCounter++;
            this.Value = Convert.ToString(rnd.Next(1, 7));

            if (timerCounter >= 200)
            {
                timer.Stop();
                this.isRolling = false;
                if (this.targetValue != -1)
                {
                    this.Value = Convert.ToString(targetValue);
                }
                this.OnRollingFinished(new RollingDiceEventArgs(Convert.ToInt32(this.Value)));
            }
        }

        /// <summary>
        /// Rolls the dice by starting the rolling timer
        /// </summary>
        public void Roll()
        {
            this.rnd = new Random(DateTime.Now.Millisecond);
            timer.Start();
        }

        /// <summary>
        /// Rolls the dice by starting the rolling timer
        /// </summary>
        /// <param name="value">The value to be displayed after rolling</param>
        public void Roll(int value)
        {
            this.rnd = new Random(DateTime.Now.Millisecond);
            this.targetValue = value;
            timer.Start();
        }

        /// <summary>
        /// Sets the displayd spots
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            this.Value = Convert.ToString(value);
        }

        /// <summary>
        /// Resets the displayed value to "?"
        /// </summary>
        public void ResetValue()
        {
            blinkCounter = 0;
            blinkState = false;
            timerCounter = 0;
            this.Value = "?";
        }

        /// <summary>
        /// Checks if the dice is clicked
        /// </summary>
        /// <param name="gs">The given GestureSample</param>
        public void CheckClick(GestureSample gs)
        {
            if (!this.ReadOnly && this.Value.Equals("?"))
            {
                int x = Convert.ToInt32(gs.Position.X);
                int y = Convert.ToInt32(gs.Position.Y);
                if (this.Rectangle.Contains(x, y))
                {
                    this.OnClick(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Starts the blink animation with the given color
        /// </summary>
        /// <param name="BlinkColor">The color</param>
        public void Blink(Texture2D BlinkColor)
        {
            this.blinkColor = BlinkColor;
            blinkTimer = new DispatcherTimer();
            blinkTimer.Interval = new TimeSpan(0,0,0,0,800);
            blinkTimer.Tick += new EventHandler(blinkTimer_Tick);
            blinkTimer.Start();
        }

        /// <summary>
        /// Is called when the blinking timer ticks.
        /// Toggles the current blinkstate.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event arguments</param>
        void blinkTimer_Tick(object sender, EventArgs e)
        {
            blinkState = !blinkState;
            blinkCounter++;
            if (blinkCounter > 5)
            {
                blinkTimer.Stop();                
                blinkState = false;
                this.OnBlinkCompleted(new EventArgs());
            }
        }

        /// <summary>
        /// Fires when the rolling of a dice is finished
        /// </summary>
        /// <param name="e">The dice event arguments</param>
        protected virtual void OnRollingFinished(RollingDiceEventArgs e)
        {
            EventHandler<RollingDiceEventArgs> handler = RollingFinish;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        /// <summary>
        /// Fires when the dice is clicked
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
        /// Fires when the blink animation of the dice is finished
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected virtual void OnBlinkCompleted(EventArgs e)
        {
            EventHandler<EventArgs> handler = BlinkComplete;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        /// <summary>
        /// Draws the dice to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (blinkState)
            {
                spriteBatch.Draw(blinkColor, this.Rectangle, Color.White);
            }
            else
            {
                if (this.ReadOnly)
                    spriteBatch.Draw(TextureManager.Gray, this.Rectangle, Color.White);
                else
                    spriteBatch.Draw(TextureManager.White, this.Rectangle, Color.White);
            }
            spriteBatch.Draw(TextureManager.DarkGray, new Rectangle(Convert.ToInt32(Rectangle.X), Convert.ToInt32(Rectangle.Y), 4, Convert.ToInt32(Rectangle.Height)), Color.White); // Left
            spriteBatch.Draw(TextureManager.DarkGray, new Rectangle(Convert.ToInt32(Rectangle.X + Rectangle.Width), Convert.ToInt32(Rectangle.Y), 4, Convert.ToInt32(Rectangle.Height) + 4), Color.White); // Right
            spriteBatch.Draw(TextureManager.DarkGray, new Rectangle(Convert.ToInt32(Rectangle.X), Convert.ToInt32(Rectangle.Y), Convert.ToInt32(Rectangle.Width), 4), Color.White); // Top
            spriteBatch.Draw(TextureManager.DarkGray, new Rectangle(Convert.ToInt32(Rectangle.X), Convert.ToInt32(Rectangle.Y + Rectangle.Height), Convert.ToInt32(Rectangle.Width) + 4, 4), Color.White); // Bottom

            Vector2 textSize = FontManager.DiceFont.MeasureString(this.Value);
            Vector2 textCenter = new Vector2((this.Rectangle.Left + this.Rectangle.Width / 2) - textSize.X / 2, (this.Rectangle.Y + this.Rectangle.Height/2) - textSize.Y / 2);
            Vector2 textSize2 = FontManager.ButtonFont.MeasureString(this.Caption);
            Vector2 textCenter2 = new Vector2((this.Rectangle.Left + this.Rectangle.Width / 2) - textSize2.X / 2, this.Rectangle.Bottom + 10);
            spriteBatch.DrawString(FontManager.DiceFont, this.Value, textCenter, Color.Black);
            spriteBatch.DrawString(FontManager.ButtonFont, Caption, textCenter2, Color.White);
        }
    }
}
