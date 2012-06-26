using Microsoft.Xna.Framework.Audio;

namespace Schiffchen.Resources
{
    /// <summary>
    /// Handles all used sounds in the XNA part of the game
    /// </summary>
    public class SoundManager
    {
        public static SoundEffect SoundWater;
        public static SoundEffect SoundExplosion;

        /// <summary>
        /// Loads all content
        /// </summary>
        /// <param name="Content">The ContentManager</param>
        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            SoundWater = Content.Load<SoundEffect>("background\\splash");
            SoundExplosion = Content.Load<SoundEffect>("background\\explosion");
        }
    }
}
