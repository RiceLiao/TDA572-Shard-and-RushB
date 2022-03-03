/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/


namespace Shard
{

    class UIText : UIBase
    {   
        private string text;
        private int size;
        private int r;
        private int g;
        private int b;
        private string fontPath;
        public string Text { get => text; set => text = value; }
        public int Size { get => size; set => size = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public string FontPath { get => fontPath; set => fontPath = value; }

        public override void initialize()
        {

        }

        public UIText(string text, float x, float y, int size, int r, int g, int b, string fontPath)
        {
            this.Text = text;
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Size = size;
            this.R = r;
            this.G = g;
            this.B = b;
            this.fontPath = fontPath;
        }

        public override void handleInput(InputEvent inp, string eventType)
        {

        }

        public override void update()
        {
            if (!Attaching)
                Bootstrap.getDisplay().showText(text, Transform.X, Transform.Y, size, r, g, b, fontPath);
        }
    }
}
