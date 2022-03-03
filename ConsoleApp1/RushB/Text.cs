using Shard;

namespace Shard
{
    class Text
    {
        private string content;
        private int r;
        private int g;
        private int b;

        public Text(string content, int r, int g, int b)
        {
            this.content = content;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public string Content { get => content; set => content = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
    }
}
