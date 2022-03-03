/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    abstract class Sound        
    {
        public abstract void playMusic(string soundPath);
        public abstract void pauseMusic();
        public abstract void playSound(string soundPath);
    }
}
