/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Media;
using System.Runtime.Versioning;

namespace Shard
{
    [SupportedOSPlatform("windows")]
    class SoundBeep : Sound
    {
        private SoundPlayer sound;
        public override void pauseMusic()
        {
            Debug.Log(sound.SoundLocation);
        }

        public override void playMusic(string soundPath)
        {
            sound = new SoundPlayer(soundPath);
            sound.Play();
        }

        public override void playSound(string soundPath)
        {
            throw new NotImplementedException();
        }

    }
}
