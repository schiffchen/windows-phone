using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using Microsoft.Xna.Framework;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Net.XMPP;
using Schiffchen.GameElemens;
using Schiffchen.Controls;
using Microsoft.Xna.Framework.Graphics;
using Schiffchen.Resources;

namespace Schiffchen.Logic
{
    /// <summary>
    /// Defines the cache of the game, where many global variables are stored
    /// </summary>
    public class AppCache
    {
        /// <summary>
        /// It's the XMPP-Manager for handling the connection
        /// </summary>
        public static XMPPManager XmppManager;

        /// <summary>
        /// Represents a current match
        /// </summary>
        public static Match CurrentMatch;

        public static System.Windows.Media.SolidColorBrush cRed;
        public static  System.Windows.Media.SolidColorBrush cGreen;
        public static System.Windows.Media.SolidColorBrush cYellow;

        /// <summary>
        /// Represents the touched ship
        /// </summary> 
        public static Ship TouchedShip;

        /// <summary>
        /// Represents the selected ship
        /// </summary>
        public static Ship ActivePlacementShip;


        /// <summary>
        /// Is called, when a static instance of AppCache is created
        /// </summary>
        static AppCache()
        {
            cRed = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 190, 0, 0));
            cGreen = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 68, 140, 0));
            cYellow = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 224, 98));

        }

        /// <summary>
        /// Draws all cached static things to the screen
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch for drawing</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            #region GameInfo

            if (AppCache.CurrentMatch!= null) {
                switch (AppCache.CurrentMatch.MatchState)
                {
                    case Enum.MatchState.ShipPlacement:
                        spriteBatch.DrawString(FontManager.GameFont, "Place your ships, by moving\n them to the playground", DeviceCache.RightOfMinimap, Microsoft.Xna.Framework.Color.White);
                        break;
                    case Enum.MatchState.Dicing:
                        spriteBatch.DrawString(FontManager.GameFont, "Roll the dice to determine,\n who begins", DeviceCache.RightOfMinimap, Microsoft.Xna.Framework.Color.White);
                        break;
                    case Enum.MatchState.Playing:
                        switch (Partner.OnlineState)
                        {
                            case Enum.PartnerState.Online:
                                spriteBatch.Draw(TextureManager.SymbolOnline, new Vector2(DeviceCache.RightOfMinimap.X, DeviceCache.RightOfMinimap.Y), Microsoft.Xna.Framework.Color.White);
                                break;
                            case Enum.PartnerState.Waiting:
                                spriteBatch.Draw(TextureManager.SymbolWaiting, new Vector2(DeviceCache.RightOfMinimap.X, DeviceCache.RightOfMinimap.Y), Microsoft.Xna.Framework.Color.White);
                                break;
                            case Enum.PartnerState.Offline:
                                spriteBatch.Draw(TextureManager.SymbolOffline, new Vector2(DeviceCache.RightOfMinimap.X, DeviceCache.RightOfMinimap.Y), Microsoft.Xna.Framework.Color.White);
                                break;
                        }
                            spriteBatch.DrawString(FontManager.GameFont, AppCache.CurrentMatch.PartnerJID.BareJID, new Vector2(DeviceCache.RightOfMinimap.X + 36, DeviceCache.RightOfMinimap.Y), Microsoft.Xna.Framework.Color.White);

                        if (AppCache.CurrentMatch.IsMyTurn)
                        {                            
                            spriteBatch.DrawString(FontManager.InfoFont, "Your Turn!", new Vector2(DeviceCache.RightOfMinimap.X, DeviceCache.RightOfMinimap.Y+50), Microsoft.Xna.Framework.Color.LightBlue);
                        }
                        else
                        {
                            spriteBatch.DrawString(FontManager.InfoFont, "Partner's Turn!", new Vector2(DeviceCache.RightOfMinimap.X, DeviceCache.RightOfMinimap.Y + 50), Microsoft.Xna.Framework.Color.Red);
                        }
                        break;
                }
            }
            

            #endregion
        }

        /// <summary>
        /// Updates all cached static things
        /// </summary>
        public static void Update()
        {
            if (AppCache.CurrentMatch != null)
                AppCache.CurrentMatch.Update();
        }
        
    }


}
