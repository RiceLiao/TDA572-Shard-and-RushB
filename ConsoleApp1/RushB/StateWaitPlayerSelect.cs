/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/
using System;
using Shard;
using System.Collections.Generic;

namespace RushB
{
    class StateWaitPlayerSelect: FSMState
    {
        public StateWaitPlayerSelect()
        {
            Transitions = new List<FSMTransition>();
        }
        public override void onEnter()
        {
            Debug.Log("onEnter " + this);
            GameRushB.isAITurn = false;
            GameRushB.messageWindow.addText(System.DateTime.Now + " " + "PLAYER TURN", 255, 0, 0);
            if (GameRushB.allIsUnavailable(1))
            {
                GameRushB.availableSoldier(1);
            }

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
                Soldier selectedSoldier = GameRushB.isPlayerSoldier(inp.X, inp.Y);
                if (selectedSoldier != null && selectedSoldier.Available)
                {
                    GameRushB.playSound("Asset/Sounds/ok.wav");
                    GameRushB.messageWindow.addText(System.DateTime.Now + " " + selectedSoldier.Name + " is selected.", 255, 255, 255);
                    GameRushB.replaceActionButton(selectedSoldier);
                    foreach (FSMTransition transition in Transitions)
                    {
                        if(transition is TransitionWaitToSelect)
                        {
                            transition.IsValid = true;
                        }
                    }
                }
            }
        }
    }
}

