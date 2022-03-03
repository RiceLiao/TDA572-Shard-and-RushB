using SDL2;
using Shard;
using System.Collections.Generic;
using System;

namespace RushB
{
    class Soldier : GameObject, InputListener
    {
        private float speed;
        private int tileX;
        private int tileY;
        private float targetX;
        private float targetY;
        private bool moving;
        private Stack<Grid> path;
        private Sprite sprite;
        private bool available;
        
        private int health;
        private int armor;
        private int foot;
        private int damage;
        private int restFoot;
        private string name;
        private List<Action> actions;
        private int sideFlag;

        public float Speed { get => speed; set => speed = value; }
        public int TileX { get => tileX; set => tileX = value; }
        public int TileY { get => tileY; set => tileY = value; }
        public float TargetX { get => targetX; set => targetX = value; }
        public float TargetY { get => targetY; set => targetY = value; }
        public bool Moving { get => moving; set => moving = value; }
        public Stack<Grid> Path { get => path; set => path = value; }
        public int Health { get => health; set => health = value; }
        public int Armor { get => armor; set => armor = value; }
        public int Foot { get => foot; set => foot = value; }
        public int Damage { get => damage; set => damage = value; }
        public int RestFoot { get => restFoot; set => restFoot = value; }
        internal Sprite Sprite { get => sprite; set => sprite = value; }
        public string Name { get => name; set => name = value; }
        public List<Action> Actions { get => actions; set => actions = value; }
        public int SideFlag { get => sideFlag; set => sideFlag = value; }
        public bool Available { get => available; set => available = value; }

        public Soldier(float x, float y, int w, int h, string spritePath, float speed)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = w;
            this.Transform.Ht = h;
            this.Transform.SpritePath = spritePath;
            this.Speed = speed;
            this.Moving = false;
            this.Path = new Stack<Grid>();

            this.TargetX = Transform.X;
            this.TargetY = Transform.Y;

            TileX = (int)((Transform.X - GameRushB.mapX) / GameRushB.tileSize);
            TileY = (int)((Transform.Y - GameRushB.mapY) / GameRushB.tileSize);
        }

        public void setSoldier(int health, int armor, int foot, int damage, string name, int sideFlag)
        {
            Health = health;
            Armor = armor;
            Foot = foot;
            Damage = damage;
            Name = name;
            SideFlag = sideFlag;
            Available = true;
        }

        public override void initialize()
        {
        }

        public void handleInput(InputEvent inp, string eventType)
        {
        }

        public override void physicsUpdate()
        {

        }

        public override void update()
        {
            if(sprite != null)
            {
                if(available)
                    Transform.SpritePath = Sprite.getSpritePath();
            }
            //Moving and update tileIndex
            if (Moving)
            {
                move(TargetX, TargetY);
                TileX = (int)((Transform.X - GameRushB.mapX) / GameRushB.tileSize);
                TileY = (int)((Transform.Y - GameRushB.mapY) / GameRushB.tileSize);
            }
            if(Moving && Math.Abs(Transform.X - TargetX) == 0f && Math.Abs(Transform.Y - TargetY) == 0f)
            {
                if (path.Count == 0 || restFoot == 0)
                {
                    Moving = false;
                    path.Clear();
                    GameRushB.addWallToMap(2, TileX, TileY);
                    if(Sprite != null)
                        Sprite.SpritePaths = GameRushB.sprites["heroIdle"];
                }
                else if (path.Count > 0)
                {
                    restFoot--;
                    Grid target = path.Pop();
                    TargetX = target.x;
                    TargetY = target.y;
                }
            }
            
            
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

        private void move(float targetX, float targetY)
        {
            if (this.Transform.X - targetX > Speed)
                this.Transform.X = this.Transform.X - Speed;
            else if (targetX - this.Transform.X > Speed)
                this.Transform.X = this.Transform.X + Speed;
            else if (this.Transform.X - targetX < Speed || targetX - this.Transform.X < Speed)
                this.Transform.X = targetX;
            if (this.Transform.Y - targetY > Speed)
                this.Transform.Y = this.Transform.Y - Speed;
            else if (targetY - this.Transform.Y > Speed)
                this.Transform.Y = this.Transform.Y + Speed;
            else if (this.Transform.Y - targetY < Speed || targetY - this.Transform.Y < Speed)
                this.Transform.Y = targetY;
        }

        public bool action(int actionIndex, Soldier target)
        {
            RushB.Action action = actions[actionIndex];
            Debug.Log(this.name + " " + action.Name + " to " + target.name);
            // EnemyTarget
            if (action.Type == 1)
            {
                if (this.sideFlag != target.sideFlag)
                {
                    if (action.Value > target.armor)
                    {
                        target.health -= action.Value - target.armor;
                    }
                    Debug.Log("target.health = "+ target.health);
                    return true;
                }
            }
            // SelfTarget
            else if (action.Type == 2)
            {
                if(target == this)
                {
                    target.armor += action.Value;
                    return true;
                }

            }
            // FriendTarget
            else if (action.Type == 3)
            {
                if(this.sideFlag == target.sideFlag)
                {
                    target.health += action.Value;
                    return true;
                }
            }

            return false;
        }
    }
}
