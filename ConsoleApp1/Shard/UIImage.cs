/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/


namespace Shard
{

    class UIImage : UIBase
    {
        public override void initialize()
        {

        }

        public UIImage(float x, float y, int w, int h, string spritePath)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = w;
            this.Transform.Ht = h;
            this.Transform.SpritePath = spritePath;
        }

        public override void handleInput(InputEvent inp, string eventType)
        {

        }

        public override void update()
        {
            if(!Attaching)
                Bootstrap.getDisplay().addToDraw(this);
        }
    }
}
