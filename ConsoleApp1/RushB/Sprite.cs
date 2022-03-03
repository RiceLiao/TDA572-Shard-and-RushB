using Shard;
using System.Collections.Generic;

namespace RushB
{
    class Sprite
    {
        // 10 Frame
        private int interval = 100;
        private List<string> spritePaths;
        private long lastUpdate;
        private int spriteCount;
        private int currentSprite;
        private List<string> originalSpritePaths;
        private int countForAction;
        private bool oneTimeAction;
        private bool oneTimeActionIsDone;

        public Sprite(List<string> spritePaths)
        {
            this.SpritePaths = spritePaths;
            this.LastUpdate = Bootstrap.getCurrentMillis();
            spriteCount = spritePaths.Count;
            currentSprite = 0;
            oneTimeAction = false;
            countForAction = 0;
            OneTimeActionIsDone = false;
        }

        public int Interval { get => interval; set => interval = value; }
        public List<string> SpritePaths 
        { 
            get => spritePaths;
            set 
            {
                currentSprite = 0;
                this.spritePaths = value;
                spriteCount = value.Count;
            } 
        }
        public long LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public bool OneTimeActionIsDone { get => oneTimeActionIsDone; set => oneTimeActionIsDone = value; }

        public string getSpritePath()
        {
            if(Bootstrap.getCurrentMillis() - lastUpdate > interval)
            {
                lastUpdate = Bootstrap.getCurrentMillis();
                if (currentSprite >= spriteCount - 1)
                    currentSprite = 0;
                else
                {
                    currentSprite++;
                    if (oneTimeAction)
                    {
                        if (countForAction == 0)
                        {
                            oneTimeAction = false;
                            spritePaths = originalSpritePaths;
                            OneTimeActionIsDone = true;
                            currentSprite = 0;
                            spriteCount = spritePaths.Count;
                        }
                        countForAction--;
                    }
                }
            }
            return spritePaths[currentSprite];
        }

        public void play(List<string> newSprite)
        {
            originalSpritePaths = spritePaths;
            spritePaths = newSprite;
            countForAction = newSprite.Count;
            oneTimeAction = true;
            currentSprite = 0;
            spriteCount = newSprite.Count;
        }
    }
}
