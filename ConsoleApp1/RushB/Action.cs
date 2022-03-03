/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    public class Action
    {
        private string spritePath;
        private int type; // 1 - EnemyTarget, 2 - SelfTarget, 3 - FriendTarget
        private string name;
        private int value;

        public Action(string spritePath, int type, string name, int value)
        {
            this.SpritePath = spritePath;
            this.Type = type;
            this.Name = name;
            this.value = value;
        }

        public string SpritePath { get => spritePath; set => spritePath = value; }
        public int Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public int Value { get => value; set => this.value = value; }
    }
}

