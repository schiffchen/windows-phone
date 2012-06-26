using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Schiffchen.Controls;
using Schiffchen.GameElemens;

namespace Schiffchen.Logic
{
    /// <summary>
    /// Handles all touch methods of the game
    /// </summary>
    static class TouchManager
    {
        /// <summary>
        /// Sets the allowed touch gestures
        /// </summary>
        public static void SetGame()
        {
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.DragComplete | GestureType.Tap;
        }

        /// <summary>
        /// Gets the current touch points
        /// </summary>
        /// <returns>A touch point coordinate</returns>
        public static Vector2? GetTouchpoints()
        {
            if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.Tap)
                {
                    return gs.Position;
                }
            }
            return null;
        }

        /// <summary>
        /// Handles the selection of a ship, when it's touched for moving
        /// </summary>
        /// <param name="gs">The GestureSample</param>
        private static void HandleShipSelection(GestureSample gs)
        {
            if (AppCache.CurrentMatch != null)
            {
                if (AppCache.CurrentMatch.MatchState == Enum.MatchState.ShipPlacement)
                {
                    Point p = new Point(Convert.ToInt32(gs.Position.X), Convert.ToInt32(gs.Position.Y));
                    foreach (Ship s in AppCache.CurrentMatch.OwnShips)
                    {
                            if (s.Rectangle.Contains(p))
                            {
                                if (!s.IsPlaced)
                                {
                                    if (!s.isTouched)
                                    {
                                        VibrationManager.Vibration.Start(new TimeSpan(0, 0, 0, 0, 100));
                                        AppCache.ActivePlacementShip = s;
                                    }
                                    s.isTouched = true;
                                    AppCache.TouchedShip = s;
                                }
                            }     
                        }
                    }
                }
            }


        /// <summary>
        /// Starts the placement and movement of a ship, when it's touched for moving
        /// </summary>
        /// <param name="gs">The GestureSample</param>
        private static void HandleShipTouchment(GestureSample gs)
        {
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
                                if (!s.IsPlaced)
                                {
                                    found = true;
                                    if (!s.isTouched)
                                    {
                                        s.StartMovement();
                                        VibrationManager.Vibration.Start(new TimeSpan(0, 0, 0, 0, 100));
                                        AppCache.ActivePlacementShip = s;
                                    }
                                    s.isTouched = true;
                                    s.Position += gs.Delta;
                                    AppCache.TouchedShip = s;
                                }
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
        }

        /// <summary>
        /// Checks all touchpoints at each call
        /// </summary>
        /// <param name="gameTime">The GameTimerEventArgs</param>
        public static void checkTouchpoints(GameTimerEventArgs gameTime)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();

                switch (gs.GestureType)
                {
                    case GestureType.FreeDrag:
                        HandleShipTouchment(gs);
                        break;
                    case GestureType.DragComplete:
                        if (AppCache.CurrentMatch != null)
                        {
                            if (AppCache.CurrentMatch.MatchState == Enum.MatchState.ShipPlacement)
                            {
                                Point p = new Point(Convert.ToInt32(gs.Position.X), Convert.ToInt32(gs.Position.Y));
                                foreach (Ship s in AppCache.CurrentMatch.OwnShips)
                                {
                                    if (s.isTouched)
                                    {
                                        s.GlueToFields();
                                        AppCache.CurrentMatch.OwnPlayground.Refresh();
                                        s.isTouched = false;
                                        VibrationManager.Vibration.Start(new TimeSpan(0, 0, 0, 0, 100));
                                    }
                                }
                            }
                        }
                        break;
                    case GestureType.Tap:
                        
                        #region Buttons
                        foreach (IconButton b in AppCache.CurrentMatch.FooterMenu.Buttons)
                        {
                            if (b != null)
                                b.CheckClick(gs);
                        }

                        for (int i = 0; i < AppCache.CurrentMatch.FooterMenu.Dices.Length; i++)
                        {
                            if (AppCache.CurrentMatch.FooterMenu.Dices[i] != null)
                            {
                                AppCache.CurrentMatch.FooterMenu.Dices[i].CheckClick(gs);
                            }
                        }
                        AppCache.CurrentMatch.OwnPlayground.CheckClick(gs);
                        AppCache.CurrentMatch.ShootingPlayground.CheckClick(gs);

                        #region Ships
                        HandleShipSelection(gs);
                        #endregion

                    #endregion
                            break;
                }

            }
        }

     

    }
}