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
    class StatePlayerSelected : FSMState
    {
        public StatePlayerSelected()
        {
            Transitions = new List<FSMTransition>();
        }
        public override void onEnter()
        {
            Debug.Log("onEnter " + this);
        }
        public override void onUpdate()
        {
        }
        public override void onExit()
        {
            Debug.Log("onExit " + this);
        }

        public override void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseDown" && inp.Button == 1)
            {
                if (GameRushB.isEndTurn)
                {
                    foreach (FSMTransition transition in Transitions)
                    {
                        if (transition is TransitionEndTurnToAI)
                        {
                            transition.IsValid = true;
                            GameRushB.isEndTurn = false;
                            return;
                        }
                    }
                }

                if (!GameRushB.isWall(1, inp.X, inp.Y))
                {
                    GameRushB.playSound("Asset/Sounds/onMayWay.wav");
                    GameRushB.setPath(GameRushB.selectedSoldier, inp.X, inp.Y);
                    GameRushB.selectedSoldier.Sprite.SpritePaths = GameRushB.sprites["heroRun"];
                    GameRushB.messageWindow.addText(System.DateTime.Now + " " +
                        GameRushB.selectedSoldier.Name + " Move to " + GameRushB.PixelXYToTileXY(inp.X, inp.Y), 255, 255, 255);
                    foreach (FSMTransition transition in Transitions)
                    {
                        if (transition is TransitionSelectToSelectAction)
                        {
                            transition.IsValid = true;
                        }
                    }
                }
            }
        }
    }
}

