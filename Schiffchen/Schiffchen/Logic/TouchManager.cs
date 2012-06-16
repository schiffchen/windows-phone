using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Schiffchen.GameElemens;

namespace Schiffchen.Logic
{
    static class TouchManager
    {

        public static void SetGame()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete;
        }


        public static void checkTouchpoints(GameTimerEventArgs gameTime)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();

                switch (gs.GestureType)
                {
                    case GestureType.FreeDrag:
                        if (AppCache.CurrentMatch != null)
                        {
                            if (AppCache.CurrentMatch.MatchState == Enum.MatchState.ShipPlacement)
                            {
                                Point p = new Point(Convert.ToInt32(gs.Position.X), Convert.ToInt32(gs.Position.Y));
                                Boolean found = false;
                                foreach (Ship s in AppCache.CurrentMatch.OwnShips)
                                {
                                    if (AppCache.TouchedShip == null || AppCache.TouchedShip == s)
                                    {
                                        if (s.Rectangle.Contains(p))
                                        {
                                            found = true;
                                            if (!s.isTouched)
                                            {
                                                s.StartMovement();
                                            }
                                            s.isTouched = true;
                                            s.Position += gs.Delta;
                                            AppCache.TouchedShip = s;
                                        }
                                        else
                                        {
                                            s.isTouched = false;
                                        }
                                    }
                                }
                                if (!found)
                                {                                   
                                    AppCache.TouchedShip = null;
                                }
                            }
                        }
                        break;
                    case GestureType.DragComplete:
                        if (AppCache.CurrentMatch != null)
                        {
                            if (AppCache.CurrentMatch.MatchState == Enum.MatchState.ShipPlacement)
                            {
                                Point p = new Point(Convert.ToInt32(gs.Position.X), Convert.ToInt32(gs.Position.Y));
                                foreach (Ship s in AppCache.CurrentMatch.OwnShips)
                                {
                                    s.GlueToFields();
                                    s.isTouched = false;
                                }
                            }
                        }
                        break;
                }

            }
        }

    }
}