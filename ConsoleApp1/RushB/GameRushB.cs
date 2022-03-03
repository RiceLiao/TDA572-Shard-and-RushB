using RushB;
using System;
using System.Collections.Generic;

namespace Shard
{
    class GameRushB : Game, InputListener
    {
        static string fontPath;
        public static SquareGraph graph;
        static AStarSearch aStar;
        static Stack<Grid> path;
        public static List<Soldier> playerSoldiers;
        public static List<Soldier> enemySoldiers;
        public static Dictionary<int, UIButton> actions;
        public static Dictionary<string, List<string>> sprites;

        //control in the turn
        public static Soldier selectedSoldier;
        public static int selectedAction;
        FiniteStateMachine turnFSM;
        public static bool isAITurn;

        //Map parameters
        public static int mapX = 160;
        public static int mapY = 20;
        public static int tileSize = 64;
        public static int mapSizeX = 15;
        public static int mapSizeY = 10;

        //UI
        UIPanel panelPS1, panelPS2, panelPS3, panelES1, panelES2, panelES3;
        UIImage avatarPS1, avatarPS2, avatarPS3, avatarES1, avatarES2, avatarES3;
        UIText textHealthPS1, textHealthPS2, textHealthPS3, textHealthES1, textHealthES2, textHealthES3;
        UIText textDamagePS1, textDamagePS2, textDamagePS3, textDamageES1, textDamageES2, textDamageES3;
        UIText textArmorPS1, textArmorPS2, textArmorPS3, textArmorES1, textArmorES2, textArmorES3;
        UIButton action1, action2, action3;
        UIButton btnEndTurn, btnEndGame;
        public static UIScrollWindow messageWindow;
        UIPanel stopPanel;

        //Game Control
        bool isStop;
        public static bool isEndTurn;

        public override void update()
        {

            Bootstrap.getDisplay().showText("FPS: " + Bootstrap.getFPS(), 10, 5, 12, 255, 255, 255, fontPath);
            turnFSM.update();
            updateUI();
        }

        public override void initialize()
        {
            initSprite();
            Bootstrap.getInput().addListener(this);
            initMap();
            initSoldiers();
            initUI();
            path = new Stack<Grid>();

            turnFSM = new FiniteStateMachine();
            isAITurn = false;
            isStop = false;

            Bootstrap.getSound().playMusic("Asset/Sounds/01.CounterMeasures.wav");
        }

        public static void playSound(string soundPath)
        {
            Bootstrap.getSound().playSound(soundPath);
        }

        public override int getTargetFrameRate()
        {
            return 60;
        }

        public void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseDown" && inp.Button == 1)
            {
                //Bootstrap.getSound().pauseMusic();
            }
            else if (eventType == "MouseDown" && inp.Button == 3)
            {
                //For test
                messageWindow.addText(System.DateTime.Now + " " + "test", 255, 255, 0);
                Bootstrap.getSound().pauseMusic();
            }
        }

        public static void setPath(Soldier soldier, int x, int y, int foot=-1)
        {

            int sourceTileX = ((int)soldier.Transform.X - mapX) / tileSize;
            int sourceTileY = ((int)soldier.Transform.Y - mapY) / tileSize;
            int targetTileX = (x - mapX) / tileSize;
            int targetTileY = (y - mapY) / tileSize;
            removeWallFromMap(2, sourceTileX, sourceTileY);
            if (targetTileX >= 0 && targetTileX <= mapSizeX 
                && targetTileY >= 0 && targetTileY <= mapSizeY && graph.Passable(new Grid(targetTileX, targetTileY)))
            {
                soldier.Path.Clear();
                Grid targetGrid = new Grid(targetTileX, targetTileY);
                Grid sourceGrid = new Grid(sourceTileX, sourceTileY);
                aStar = new AStarSearch(graph, sourceGrid, targetGrid);
                Grid currentGrid = targetGrid;
                
                soldier.Path.Push(new Grid(currentGrid.x * tileSize + mapX, currentGrid.y * tileSize + mapY));
                while (true)
                {
                    currentGrid = aStar.cameFrom[currentGrid];
                    if (currentGrid == sourceGrid)
                        break;
                    soldier.Path.Push(new Grid(currentGrid.x * tileSize + mapX, currentGrid.y * tileSize + mapY));
                }
                soldier.RestFoot = soldier.Foot;
                soldier.Moving = true;
            }
        }

        public static Soldier isPlayerSoldier(int x, int y)
        {
            int tileX = (int)((x - mapX) / tileSize);
            int tileY = (int)((y - mapY) / tileSize);
            foreach (Soldier soldier in playerSoldiers)
            {
                if(tileX == soldier.TileX && tileY == soldier.TileY)
                {
                    selectedSoldier = soldier;
                    return selectedSoldier;
                }
            }
            return null;
        }

        public static Soldier isEnemySoldier(int x, int y)
        {
            int tileX = (int)((x - mapX) / tileSize);
            int tileY = (int)((y - mapY) / tileSize);
            foreach (Soldier soldier in enemySoldiers)
            {
                if (tileX == soldier.TileX && tileY == soldier.TileY)
                {
                    return soldier;
                }
            }
            return null;
        }

        public static bool allIsUnavailable(int sideflag)
        {
            if(sideflag == 1)
            {
                Soldier soldier = playerSoldiers.Find(soldier => soldier.Available == true);
                if (soldier == null)
                    return true;
            }
            else
            {
                Soldier soldier = enemySoldiers.Find(soldier => soldier.Available == true);
                if (soldier == null)
                    return true;
            }
            return false;
        }

        public static void availableSoldier(int sideflag)
        {
            if (sideflag == 1)
            {
                foreach(Soldier soldier in playerSoldiers)
                {
                    soldier.Available = true;
                }
            }
            else
            {
                foreach (Soldier soldier in enemySoldiers)
                {
                    soldier.Available = true;
                }
            }
        }

        public static bool isAction(int x, int y)
        {
            foreach(KeyValuePair<int, UIButton> button in actions)
            {
                Transform transform = button.Value.Transform;
                if (x > transform.X && x < transform.X + transform.Wid && y > transform.Y && y < transform.Y + transform.Ht)
                {
                    selectedAction = button.Key;
                    return true;
                }
            }
            return false;
        }

        public void initUI()
        {
            int panelWidth = 145;
            int panelHeight = 200;
            fontPath = "Asset/Arts/UIPackSpace/Fonts/kenvector_future_thin.ttf";

            //Soldier info
            panelPS1 = new UIPanel(5, mapY, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarPS1 = new UIImage(30,5, 80, 80, "Asset/Arts/HeroAvatar.png");
            textHealthPS1 = new UIText("Health : " + playerSoldiers[0].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamagePS1 = new UIText("Damage : " + playerSoldiers[0].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorPS1 = new UIText("Armor : " + playerSoldiers[0].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelPS1.attachUI(avatarPS1);
            panelPS1.attachUI(textHealthPS1);
            panelPS1.attachUI(textDamagePS1);
            panelPS1.attachUI(textArmorPS1);

            panelPS2 = new UIPanel(5, mapY + panelHeight + 10, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarPS2 = new UIImage(30, 5, 80, 80, "Asset/Arts/HeroAvatar.png");
            textHealthPS2 = new UIText("Health : " + playerSoldiers[1].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamagePS2 = new UIText("Damage : " + playerSoldiers[1].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorPS2 = new UIText("Armor : " + playerSoldiers[1].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelPS2.attachUI(avatarPS2);
            panelPS2.attachUI(textHealthPS2);
            panelPS2.attachUI(textDamagePS2);
            panelPS2.attachUI(textArmorPS2);

            panelPS3 = new UIPanel(5, mapY + (panelHeight + 10) * 2, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarPS3 = new UIImage(30, 5, 80, 80, "Asset/Arts/HeroAvatar.png");
            textHealthPS3 = new UIText("Health : " + playerSoldiers[2].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamagePS3 = new UIText("Damage : " + playerSoldiers[2].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorPS3 = new UIText("Armor : " + playerSoldiers[2].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelPS3.attachUI(avatarPS3);
            panelPS3.attachUI(textHealthPS3);
            panelPS3.attachUI(textDamagePS3);
            panelPS3.attachUI(textArmorPS3);

            panelES1 = new UIPanel(1130, mapY, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarES1 = new UIImage(30, 5, 80, 80, "Asset/Arts/AIAvatar.png");
            textHealthES1 = new UIText("Health : " + enemySoldiers[0].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamageES1 = new UIText("Damage : " + enemySoldiers[0].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorES1 = new UIText("Armor : " + enemySoldiers[0].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelES1.attachUI(avatarES1);
            panelES1.attachUI(textHealthES1);
            panelES1.attachUI(textDamageES1);
            panelES1.attachUI(textArmorES1);

            panelES2 = new UIPanel(1130, mapY + panelHeight + 10, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarES2 = new UIImage(30, 5, 80, 80, "Asset/Arts/AIAvatar.png");
            textHealthES2 = new UIText("Health : " + enemySoldiers[1].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamageES2 = new UIText("Damage : " + enemySoldiers[1].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorES2 = new UIText("Armor : " + enemySoldiers[1].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelES2.attachUI(avatarES2);
            panelES2.attachUI(textHealthES2);
            panelES2.attachUI(textDamageES2);
            panelES2.attachUI(textArmorES2);

            panelES3 = new UIPanel(1130, mapY + (panelHeight + 10) * 2, panelWidth, panelHeight, "Asset/Arts/PanelWindow.png");
            avatarES3 = new UIImage(30, 5, 80, 80, "Asset/Arts/AIAvatar.png");
            textHealthES3 = new UIText("Health : " + enemySoldiers[2].Health.ToString(), 5, 90, 15, 255, 255, 255, fontPath);
            textDamageES3 = new UIText("Damage : " + enemySoldiers[2].Damage.ToString(), 5, 110, 15, 255, 255, 255, fontPath);
            textArmorES3 = new UIText("Armor : " + enemySoldiers[2].Armor.ToString(), 5, 130, 15, 255, 255, 255, fontPath);
            panelES3.attachUI(avatarES3);
            panelES3.attachUI(textHealthES3);
            panelES3.attachUI(textDamageES3);
            panelES3.attachUI(textArmorES3);

            //Button for action and game control
            actions = new Dictionary<int, UIButton>();
            action1 = new UIButton(550, 750, 80, 80, "Asset/Arts/sharp-shuriken.png");
            action2 = new UIButton(650, 750, 80, 80, "Asset/Arts/police-badge.png");
            action3 = new UIButton(750, 750, 80, 80, "Asset/Arts/erlenmeyer.png");
            actions.Add(0, action1);
            actions.Add(1, action2);
            actions.Add(2, action3);

            btnEndTurn = new UIButton(1000, 700, 200, 80, "Asset/Arts/Btn.png");
            btnEndTurn.setText("End Turn", 18, 255, 255, 255, fontPath);
            btnEndTurn.addClickTask(endTurn);
            btnEndGame = new UIButton(1000, 780, 200, 80, "Asset/Arts/Btn.png");
            btnEndGame.setText("End Game", 18, 255, 255, 255, fontPath);
            btnEndGame.addClickTask(endGame);

            messageWindow = new UIScrollWindow(5, 700, 500, 150, "Asset/Arts/PanelWindow.png");
            messageWindow.setTextParam(12, fontPath);
        }

        public void updateUI()
        {
            textHealthPS1.Text = "Health : " + playerSoldiers[0].Health.ToString();
            textDamagePS1.Text = "Damage : " + playerSoldiers[0].Damage.ToString();
            textArmorPS1.Text = "Armor : " + playerSoldiers[0].Armor.ToString();
            textHealthPS2.Text = "Health : " + playerSoldiers[1].Health.ToString();
            textDamagePS2.Text = "Damage : " + playerSoldiers[1].Damage.ToString();
            textArmorPS2.Text = "Armor : " + playerSoldiers[1].Armor.ToString();
            textHealthPS3.Text = "Health : " + playerSoldiers[2].Health.ToString();
            textDamagePS3.Text = "Damage : " + playerSoldiers[2].Damage.ToString();
            textArmorPS3.Text = "Armor : " + playerSoldiers[2].Armor.ToString();

            textHealthES1.Text = "Health : " + enemySoldiers[0].Health.ToString();
            textDamageES1.Text = "Damage : " + enemySoldiers[0].Damage.ToString();
            textArmorES1.Text = "Armor : " + enemySoldiers[0].Armor.ToString();
            textHealthES2.Text = "Health : " + enemySoldiers[1].Health.ToString();
            textDamageES2.Text = "Damage : " + enemySoldiers[1].Damage.ToString();
            textArmorES2.Text = "Armor : " + enemySoldiers[1].Armor.ToString();
            textHealthES3.Text = "Health : " + enemySoldiers[2].Health.ToString();
            textDamageES3.Text = "Damage : " + enemySoldiers[2].Damage.ToString();
            textArmorES3.Text = "Armor : " + enemySoldiers[2].Armor.ToString();
        }

        public static void addWallToMap(int type, int x, int y)
        {
            // 1 means using pixel x and y
            if(type == 1)
            {
                int tileX = (int)((x - mapX) / tileSize);
                int tileY = (int)((y - mapY) / tileSize);
                graph.walls.Add(new Grid(tileX, tileY)); 
            }
            // 2 means using tile x and y
            else if (type == 2)
            {
                graph.walls.Add(new Grid(x, y));
            }
        }

        public static bool isNear(Grid source, Grid target)
        {
            foreach (Grid neigh in graph.FullNeighbors(source))
            {
                if (neigh == target)
                {
                    return true;
                }
            }
            return false;
        }

        public static void removeWallFromMap(int type, int x, int y)
        {
            // 1 means using pixel x and y
            if (type == 1)
            {
                int tileX = (int)((x - mapX) / tileSize);
                int tileY = (int)((y - mapY) / tileSize);
                graph.walls.Remove(new Grid(tileX, tileY));
            }
            // 2 means using tile x and y
            else if (type == 2)
            {
                graph.walls.Remove(new Grid(x, y));
            }
        }

        public static void replaceActionButton(Soldier soldier)
        {
            for(int i = 0; i<actions.Count; i++)
            {
                actions[i].Transform.SpritePath = soldier.Actions[i].SpritePath;
            }
        }

        public static Grid TileXYToPixelXY(int x, int y)
        {
            return new Grid(x * tileSize + mapX, y * tileSize + mapY);
        }

        public static Grid PixelXYToTileXY(int x, int y)
        {
            return new Grid((int)((x - mapX) / tileSize), (int)((y - mapY) / tileSize));
        }
        public static bool isWall(int type, int x, int y)
        {
            Grid grid;
            // 1 means using pixel x and y
            if (type == 1)
                grid = PixelXYToTileXY(x, y);
            else
                grid = new Grid(x, y);
            // 2 means using tile x and y
            if (graph.walls.Contains(grid))
                return true;
            return false;
        }

        public void initMap()
        {
            graph = new SquareGraph(mapSizeX, mapSizeY);
            graph.walls = new HashSet<Grid>
            {
                new Grid(4, 1), new Grid(4, 2), new Grid(4, 3), new Grid(4, 4), new Grid(4, 5), new Grid(4, 6), new Grid(4, 7),
                new Grid(5, 4), new Grid(6, 4), new Grid(8, 4), new Grid(9, 4),
                new Grid(10, 1), new Grid(10, 2), new Grid(10, 3), new Grid(10, 4), new Grid(10, 5), new Grid(10, 6), new Grid(10, 7),
            };

            for (var y = 0; y < mapSizeY; y++)
            {
                for (var x = 0; x < mapSizeX; x++)
                {
                    if (graph.Passable(new Grid(x, y)))
                    {
                        Tile tile = new Tile(mapX + tileSize * x, mapY + tileSize * y, tileSize, tileSize, "Asset/Arts/tile.png");
                    }else if (!graph.Passable(new Grid(x, y)))
                    {
                        Tile tile = new Tile(mapX + tileSize * x, mapY + tileSize * y, tileSize, tileSize, "Asset/Arts/brick.png");
                    }

                }
            }
        }

        public void initSoldiers()
        {
            playerSoldiers = new List<Soldier>();
            enemySoldiers = new List<Soldier>();

            //Add Actions
            RushB.Action fighterAction1 = new RushB.Action("Asset/Arts/stiletto.png", 1,"Cut", 10);
            RushB.Action fighterAction2 = new RushB.Action("Asset/Arts/police-badge.png", 2, "Shield", 5);
            RushB.Action fighterAction3 = new RushB.Action("Asset/Arts/erlenmeyer.png", 3, "Heal", 30);
            List<RushB.Action> fighterActions = new List<RushB.Action>();
            fighterActions.Add(fighterAction1);
            fighterActions.Add(fighterAction2);
            fighterActions.Add(fighterAction3);

            Soldier playerSoldier1 = new Soldier(mapX, mapY, tileSize, tileSize, "spaceship.png", 3f);
            Soldier playerSoldier2 = new Soldier(mapX, mapY + tileSize * 3, tileSize, tileSize, "spaceship.png", 3f);
            Soldier playerSoldier3 = new Soldier(mapX, mapY + tileSize * 6, tileSize, tileSize, "spaceship.png", 3f);
            playerSoldier1.setSoldier(100, 0, 15, 5, "Arthas", 1);
            playerSoldier2.setSoldier(100, 0, 8, 5, "Sylvanas", 1);
            playerSoldier3.setSoldier(100, 0, 8, 5, "Grom", 1);
            playerSoldier1.Actions = fighterActions;
            playerSoldier2.Actions = fighterActions;
            playerSoldier3.Actions = fighterActions;

            //Add Sprite
            playerSoldier1.Sprite = new Sprite(sprites["heroIdle"]);
            playerSoldier2.Sprite = new Sprite(sprites["heroIdle"]);
            playerSoldier3.Sprite = new Sprite(sprites["heroIdle"]);

            playerSoldiers.Add(playerSoldier1);
            playerSoldiers.Add(playerSoldier2);
            playerSoldiers.Add(playerSoldier3);
            addWallToMap(1, (int)playerSoldier1.Transform.X, (int)playerSoldier1.Transform.Y);
            addWallToMap(1, (int)playerSoldier2.Transform.X, (int)playerSoldier2.Transform.Y);
            addWallToMap(1, (int)playerSoldier3.Transform.X, (int)playerSoldier3.Transform.Y);

            Soldier enemySoldier1 = new Soldier(mapX + (mapSizeX - 1) * tileSize, mapY, tileSize, tileSize, "spaceship.png", 3f);
            Soldier enemySoldier2 = new Soldier(mapX + (mapSizeX - 1) * tileSize, mapY + tileSize * 3, tileSize, tileSize, "spaceship.png", 3f);
            Soldier enemySoldier3 = new Soldier(mapX + (mapSizeX - 1) * tileSize, mapY + tileSize * 6, tileSize, tileSize, "spaceship.png", 3f);
            enemySoldier1.setSoldier(100, 0, 8, 5, "Enemy 1", 2);
            enemySoldier2.setSoldier(100, 0, 8, 5, "Enemy 2", 2);
            enemySoldier3.setSoldier(100, 0, 8, 5, "Enemy 3", 2);
            enemySoldier1.Actions = fighterActions;
            enemySoldier2.Actions = fighterActions;
            enemySoldier3.Actions = fighterActions;

            enemySoldiers.Add(enemySoldier1);
            enemySoldiers.Add(enemySoldier2);
            enemySoldiers.Add(enemySoldier3);
            addWallToMap(1, (int)enemySoldier1.Transform.X, (int)enemySoldier1.Transform.Y);
            addWallToMap(1, (int)enemySoldier2.Transform.X, (int)enemySoldier2.Transform.Y);
            addWallToMap(1, (int)enemySoldier3.Transform.X, (int)enemySoldier3.Transform.Y);
        }

        public void stopGame()
        {
            isStop = true;
            panelPS1.Active = false;
            panelPS2.Active = false;
            panelPS3.Active = false;
            panelES1.Active = false;
            panelES2.Active = false;
            panelES3.Active = false;
            action1.Active = false;
            action2.Active = false;
            action3.Active = false;
            btnEndTurn.Active = false;
            btnEndGame.Active = false;

            stopPanel = new UIPanel(450, 20, 400, 900, "Asset/Arts/BigPanel.png");
        }

        public void resumeGame()
        {
            isStop = false;
            panelPS1.Active = true;
            panelPS2.Active = true;
            panelPS3.Active = true;
            panelES1.Active = true;
            panelES2.Active = true;
            panelES3.Active = true;
            action1.Active = true;
            action2.Active = true;
            action3.Active = true;
            btnEndTurn.Active = true;
            btnEndGame.Active = true;

            stopPanel = null;
        }

        public static void endTurn()
        {
            if (selectedSoldier != null)
                selectedSoldier.Available = false;
            isEndTurn = true;
            selectedAction = 0;
            selectedSoldier = null;
        }

        public void endGame()
        {
            System.Environment.Exit(0);
        }

        public void initSprite()
        {
            sprites = new Dictionary<string, List<string>>();

            List<string> spIdle = new List<string>();
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_0.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_1.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_2.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_3.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_4.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_5.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_6.png");
            spIdle.Add("Asset/Arts/HeroKnight/Idle/HeroKnight_Idle_7.png");
            sprites["heroIdle"] = spIdle;

            List<string> spRun = new List<string>();
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_0.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_1.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_2.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_3.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_4.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_5.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_6.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_7.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_8.png");
            spRun.Add("Asset/Arts/HeroKnight/Run/HeroKnight_Run_9.png");
            sprites["heroRun"] = spRun;

            List<string> spAttack = new List<string>();
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_0.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_1.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_2.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_3.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_4.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_5.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_6.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_7.png");
            spAttack.Add("Asset/Arts/HeroKnight/Attack3/HeroKnight_Attack3_8.png");
            sprites["heroAttack"] = spAttack;

            List<string> spBlock = new List<string>();
            spBlock.Add("Asset/Arts/HeroKnight/Block/HeroKnight_Block_0.png");
            spBlock.Add("Asset/Arts/HeroKnight/Block/HeroKnight_Block_1.png");
            spBlock.Add("Asset/Arts/HeroKnight/Block/HeroKnight_Block_2.png");
            spBlock.Add("Asset/Arts/HeroKnight/Block/HeroKnight_Block_3.png");
            spBlock.Add("Asset/Arts/HeroKnight/Block/HeroKnight_Block_4.png");
            sprites["heroBlock"] = spBlock;
        }
    }



}
