/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/
using System.Collections.Generic;

namespace Shard
{
    class UIScrollWindow : UIBase
    {
        private List<Text> texts;
        private int textSize;
        private string fontPath;
        private int offset;
        private int mouseX;
        private int mouseY;
        private int initTextY;

        public int TextSize { get => textSize; set => textSize = value; }
        public string FontPath { get => fontPath; set => fontPath = value; }
        public int Offset { get => offset; set => offset = value; }
        public List<Text> Texts { get => texts; set => texts = value; }

        public override void initialize()
        {

        }

        public UIScrollWindow(float x, float y, int w, int h, string spritePath)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = w;
            this.Transform.Ht = h;
            this.Transform.SpritePath = spritePath;
            this.Texts = new List<Text>();
        }

        public void setTextParam(int textSize, string fontPath)
        {
            this.textSize = textSize;
            this.fontPath = fontPath;
            this.initTextY = (int)Transform.Y + Transform.Ht - 20 - textSize;
        }

        public void addText(string text, int r, int g, int b)
        {
            texts.Add(new Text(text, r, g, b));
            offset = texts.Count * textSize;
        }

        public override void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseMotion")
            {
                mouseX = inp.X;
                mouseY = inp.Y;
            }

            if (eventType == "MouseWheel")
            {
                if (mouseX > this.Transform.X && mouseX < this.Transform.X + this.Transform.Wid &&
                    mouseY > this.Transform.Y && mouseY < this.Transform.Y + this.Transform.Ht)
                {
                    offset += inp.Y * 2;
                }
            }
        }


        public override void update()
        {
            if (!Attaching)
            {
                Bootstrap.getDisplay().addToDraw(this);
                if(texts.Count > 0)
                {
                    for (int i = texts.Count - 1; i >= 0; i--)
                    //for (int i = 0; i < texts.Count; i++)
                    {
                        int textX = (int)Transform.X + 15;
                        //int textY = (int)Transform.Y + Transform.Ht - 20 - offset + (i + 1) * textSize;
                        int textY = initTextY - offset + (i + 1) * textSize;
                        if (textY < (int)Transform.Y + Transform.Ht - 20 && textY > (int)Transform.Y + 10)
                            Bootstrap.getDisplay().showText(texts[i].Content, textX, textY, 15, texts[i].R, texts[i].G, texts[i].B);
                    }
                }
            }
                
        }
    }
}
