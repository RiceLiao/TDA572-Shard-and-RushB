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
    class StatePlayerSelectAction : FSMState
    {
        public StatePlayerSelectAction()
        {
            Transitions = new List<FSMTransition>();
        }
        public override void onEnter()
        {
            Debug.Log("onEnter " + this);
            //GameRushB.selectedSoldier.Sprite.SpritePaths = GameRushB.sprites["heroIdle"];
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
                if(GameRushB.isAction(inp.X, inp.Y))
                {
                    Debug.Log("isAction");
                    foreach (FSMTransition transition in Transitions)
                    {
                        if (transition is TransitionSelectActionToSelectTarget)
                        {
                            transition.IsValid = true;
                        }
                    }
                }

                if(GameRushB.isEndTurn)
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
            }
        }
    }
}

