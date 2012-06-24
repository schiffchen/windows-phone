using Microsoft.Xna.Framework.Audio;

namespace Schiffchen.Resources
{
    public class SoundManager
    {
        public static SoundEffect SoundWater;
        public static SoundEffect SoundExplosion;


        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            SoundWater = Content.Load<SoundEffect>("background\\splash");
            SoundExplosion = Content.Load<SoundEffect>("background\\explosion");
        }
    }
}
