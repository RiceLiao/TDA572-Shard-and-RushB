/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/


namespace Shard
{

    class UIButton : UIBase
    {
        private string text;
        private int textSize;
        private int finalTextSize;
        private int r;
        private int g;
        private int b;
        private string fontPath;
        private float textX;
        private float textY;

        public delegate void ClickHandle();
        private ClickHandle clickEvent;

        public string Text { get => text; set => text = value; }
        public int TextSize { get => textSize; set => textSize = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public string FontPath { get => fontPath; set => fontPath = value; }

        public override void initialize()
        {

        }

        public UIButton(float x, float y, int w, int h, string spritePath)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = w;
            this.Transform.Ht = h;
            this.Transform.SpritePath = spritePath;
        }

        public void setText(string text, int textSize, int r, int g, int b, string fontPath)
        {
            this.text = text;
            this.textSize = textSize;
            this.r = r;
            this.g = g;
            this.b = b;
            this.fontPath = fontPath;
            finalTextSize = textSize;
            textX = Transform.X + Transform.Wid / 4;
            textY = Transform.Y + Transform.Ht / 3;
        }

        public override void handleInput(InputEvent inp, string eventType)
        {
            if(Active)
            {
                if (inp.X > this.Transform.X && inp.X < this.Transform.X + this.Transform.Wid &&
                    inp.Y > this.Transform.Y && inp.Y < this.Transform.Y + this.Transform.Ht)
                {
                    if (eventType == "MouseDown")
                    {
                        this.Transform.Scalex = 1.1f;
                        this.Transform.Scaley = 1.1f;
                        if (this.clickEvent != null)
                            this.clickEvent();

                        finalTextSize = textSize + 3;
                        textX = Transform.X + Transform.Wid / 3.5f;
                    }
                    else if (eventType == "MouseUp")
                    {
                        this.Transform.Scalex = 1.0f;
                        this.Transform.Scaley = 1.0f;

                        finalTextSize = textSize;
                        textX = Transform.X + Transform.Wid / 4;
                    }
                }
            }
        }

        public void addClickTask(ClickHandle handler)
        {
            if (this.clickEvent == null)
                this.clickEvent = new ClickHandle(handler);
            else
                this.clickEvent += new ClickHandle(handler);
        }

        public void removeClickTask(ClickHandle handler)
        {
            System.Delegate.Remove(this.clickEvent, handler);
        }

        public override void update()
        {
            if (!Attaching)
            {
                Bootstrap.getDisplay().addToDraw(this);
                if(text != null)
                    Bootstrap.getDisplay().showText(text, textX, textY, finalTextSize, R, G, B, FontPath);
            }
                
        }
    }
}
