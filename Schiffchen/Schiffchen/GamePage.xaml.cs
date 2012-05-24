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

namespace Schiffchen
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Freigabemodus des Grafikgeräts zum Aktivieren des XNA-Renderings festlegen
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: Hier Spielinhalt mit this.content laden

            // Timer starten
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Timer beenden
            timer.Stop();

            // Freigabemodus des Grafikgeräts zum Aktivieren des XNA-Renderings festlegen
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Ermöglicht der Seite die Ausführung von Logik, z. B. zur Aktualisierung von Welten,
        /// Kollisionsprüfung, Eingabeerfassung und Audiowiedergabe.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Hier Aktualisierungslogik hinzufügen
        }

        /// <summary>
        /// Ermöglicht der Seite eigene Zeichenvorgänge.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Hier Zeichnungscode hinzufügen
        }
    }
}