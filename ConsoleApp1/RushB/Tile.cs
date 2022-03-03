using Shard;

namespace RushB
{
    class Tile : GameObject, InputListener
    {
        public Tile(float x, float y, int w, int h, string spritePath)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = w;
            this.Transform.Ht = h;
            this.Transform.SpritePath = spritePath;
        }

        public override void initialize() { }

        public void handleInput(InputEvent inp, string eventType) { }


        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override string ToString()
        {
            return "Tile: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
