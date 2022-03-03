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
    class StatePlayerSelectTarget : FSMState
    {
        private bool actionDone;
        public StatePlayerSelectTarget()
        {
            Transitions = new List<FSMTransition>();
        }
        public override void onEnter()
        {
            Debug.Log("onEnter " + this);
            actionDone = false;
        }
        public override void onUpdate()
        {
            if (GameRushB.selectedSoldier.Sprite.OneTimeActionIsDone)
            {
                GameRushB.selectedSoldier.Sprite.SpritePaths = GameRushB.sprites["heroIdle"];
                GameRushB.selectedSoldier.Sprite.OneTimeActionIsDone = false;
                GameRushB.selectedSoldier.Available = false;
                foreach (FSMTransition transition in Transitions)
                {
                    if (transition is TransitionSelectTargetToAI)
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
            if (eventType == "MouseDown" && inp.Button == 1)
            {
                if (GameRushB.isAction(inp.X, inp.Y))
                {
                    Debug.Log("isAction");
                    foreach (FSMTransition transition in Transitions)
                    {
                        if (transition is TransitionSelectTargetToSelectTarget)
                        {
                            transition.IsValid = true;
                            return;
                        }
                    }
                }

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

                Soldier target = null;
                Action action = GameRushB.selectedSoldier.Actions[GameRushB.selectedAction];
                if (action.Type == 1)
                {
                    target = GameRushB.isEnemySoldier(inp.X, inp.Y);
                    if (target is not null)
                    {
                        if (!GameRushB.isNear(new Grid(GameRushB.selectedSoldier.TileX, GameRushB.selectedSoldier.TileY),
                                                new Grid(target.TileX, target.TileY)))
                        {
                            GameRushB.playSound("Asset/Sounds/notHappening.wav");
                            return;
                        }
                        GameRushB.selectedSoldier.Sprite.play(GameRushB.sprites["heroAttack"]);
                        GameRushB.playSound("Asset/Sounds/gameOver.wav");
                    }
                }
                else if(action.Type == 2)
                {
                    target = GameRushB.isPlayerSoldier(inp.X, inp.Y);
                    if (target is null)
                    {
                        GameRushB.playSound("Asset/Sounds/notHappening.wav");
                    }else
                    {
                        GameRushB.playSound("Asset/Sounds/ready.wav");
                        GameRushB.selectedSoldier.Sprite.play(GameRushB.sprites["heroBlock"]);
                    }
                }
                else if (action.Type == 3)
                {
                    target = GameRushB.isPlayerSoldier(inp.X, inp.Y);
                    if (target is null)
                    {
                        GameRushB.playSound("Asset/Sounds/notHappening.wav");
                    }
                    else
                    {
                        GameRushB.playSound("Asset/Sounds/ready.wav");
                    }
                }
                if (target is not null)
                {
                    bool actionResult = GameRushB.selectedSoldier.action(GameRushB.selectedAction, target);
                    if (actionResult)
                    {
                        GameRushB.messageWindow.addText(System.DateTime.Now + " " +
                            GameRushB.selectedSoldier.Name + " " +
                            GameRushB.selectedSoldier.Actions[GameRushB.selectedAction].Name +
                        " To " + target.Name, 255, 255, 255);
                        
                    }
                }
                
            }
        }
    }
}

