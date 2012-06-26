using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Schiffchen.Resources;
using Schiffchen.GameElemens;
using Schiffchen.Logic;

namespace Schiffchen
{
    /// <summary>
    /// This is the xna game page, where a match will be rendered
    /// </summary>
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        TiledBackground background;
    

        /// <summary>
        /// Initialize all components and timers
        /// </summary>
        public GamePage()
        {
            InitializeComponent();

            // Inhalts-Manager aus der Anwendung abrufen
            contentManager = (Application.Current as App).Content;

            // Timer für diese Seite erstellen
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

        }

        /// <summary>
        /// Loads the content, fills the global variables, starts the timer
        /// </summary>
        /// <param name="e">The navigation event arguments</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Freigabemodus des Grafikgeräts zum Aktivieren des XNA-Renderings festlegen
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            DeviceCache.ScreenWidth = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width;
            DeviceCache.ScreenHeight = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height;
            TextureManager.LoadContent(contentManager, SharedGraphicsDeviceManager.Current.GraphicsDevice);
            FontManager.LoadContent(contentManager);
            background = new TiledBackground(TextureManager.GameBackground, DeviceCache.ScreenWidth, DeviceCache.ScreenHeight);


            if (AppCache.CurrentMatch != null)
            {
                AppCache.CurrentMatch.Initialize();
            }

            TouchManager.SetGame();
            // Timer starten
            timer.Start();

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Timer beenden
            timer.Stop();

            // Freigabemodus des Grafikgeräts zum Aktivieren des XNA-Renderings festlegen
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Updates all relevant updateable components
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            background.Update(new Microsoft.Xna.Framework.Rectangle(0, 0, DeviceCache.ScreenWidth, DeviceCache.ScreenHeight));
            TouchManager.checkTouchpoints(e);
            if (AppCache.CurrentMatch != null && AppCache.CurrentMatch.MatchState == Logic.Enum.MatchState.ShipPlacement && AppCache.TouchedShip != null)
            {
                if (!AppCache.TouchedShip.IsPlaced)
                {
                    CollissionManager.HandleFieldCheck(AppCache.CurrentMatch.OwnPlayground, AppCache.TouchedShip);
                }
            }
            AppCache.Update();
        }

        /// <summary>
        /// Draws all relevant drawable components
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            AppCache.CurrentMatch.Draw(spriteBatch);
            AppCache.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}