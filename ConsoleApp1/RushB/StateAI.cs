/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/
using Shard;
using System.Collections.Generic;

namespace RushB
{
    class StateAI : FSMState
    {
        private Soldier soldier;
        private Soldier targetSoldier;
        private bool doneAction;
        public StateAI()
        {
            Transitions = new List<FSMTransition>();
        }
        public override void onEnter()
        {
            Debug.Log("onEnter " + this);
            GameRushB.messageWindow.addText(System.DateTime.Now + " " + "AI TURN", 255, 0 ,0);

            doneAction = false;
            GameRushB.isAITurn = true;
            //select available soldier
            if (GameRushB.allIsUnavailable(2))
                GameRushB.availableSoldier(2);
            foreach(Soldier s in GameRushB.enemySoldiers)
            {
                if(s.Available)
                    soldier = s;
            }
            targetSoldier = GameRushB.playerSoldiers[0];
            Debug.Log(soldier.TileX + "," + soldier.TileY + " restFoot = " + soldier.RestFoot + ", Foot = "+ soldier.Foot);
            foreach (Grid neigh in GameRushB.graph.Neighbors(new Grid(targetSoldier.TileX, targetSoldier.TileY)))
            {
                Grid grid = GameRushB.TileXYToPixelXY(neigh.x, neigh.y);
                GameRushB.setPath(soldier, grid.x, grid.y);
            }
        }
        public override void onUpdate()
        {
            if (!doneAction && !soldier.Moving)
            {
                if (GameRushB.isNear(new Grid(soldier.TileX, soldier.TileY),
                                                new Grid(targetSoldier.TileX, targetSoldier.TileY)))
                {
                    soldier.action(0, targetSoldier);
                    doneAction = true;
                    GameRushB.messageWindow.addText(System.DateTime.Now + " " +
                        soldier.Name + " " + soldier.Actions[0].Name +
                        " To " + targetSoldier.Name, 255, 255, 255);
                }else
                {
                    doneAction = true;
                }
            }
            if (doneAction)
            {
                soldier.Available = false;
                foreach (FSMTransition transition in Transitions)
                {
                    if (transition is TransitionAIToWaitPlayer)
                    {
                        transition.IsValid = true;
                    }
                }
            }
        }
        public override void onExit()
        {
            Debug.Log("onExit " + this);
        }

        public override void handleInput(InputEvent inp, string eventType)
        {
        }
    }
}

