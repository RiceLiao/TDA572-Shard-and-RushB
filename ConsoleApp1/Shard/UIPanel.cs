/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/
using System.Collections.Generic;

namespace Shard
{

    class UIPanel : UIBase
    {
        private List<UIBase> attachedUI;
        public override void initialize()
        {
            attachedUI = new List<UIBase>();
        }

        public UIPanel(float x, float y, int w, int h, string spritePath)
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

        public void attachUI(UIBase aboveUI)
        {
            aboveUI.Attaching = true;
            aboveUI.Transform.X = this.Transform.X + aboveUI.Transform.X;
            aboveUI.Transform.Y = this.Transform.Y + aboveUI.Transform.Y;
            attachedUI.Add(aboveUI);
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
            foreach (UIBase aboveUI in attachedUI)
            {
                if (aboveUI is UIText)
                {
                    UIText aboveText = (UIText)aboveUI;
                    Bootstrap.getDisplay().showText(aboveText.Text, aboveText.Transform.X, aboveText.Transform.Y, aboveText.Size,
                        aboveText.R, aboveText.G, aboveText.B, aboveText.FontPath);
                }
                else
                {
                    Bootstrap.getDisplay().addToDraw(aboveUI);
                }
            }
        }
    }
}
