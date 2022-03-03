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
    class FiniteStateMachine: InputListener
    {
        private List<FSMState> states;
        private FSMState initState;
        private FSMState activeState;

        public List<FSMState> States { get => states; set => states = value; }
        public FSMState InitState { get => initState; set => initState = value; }
        public FSMState ActiveState { get => activeState; set => activeState = value; }

        public FiniteStateMachine()
        {
            states = new List<FSMState>();
            FSMState stateWaitPlayerSelect = new StateWaitPlayerSelect();
            FSMState statePlayerSelected = new StatePlayerSelected();
            FSMState statePlayerSelectAction = new StatePlayerSelectAction();
            FSMState statePlayerSelectTarget = new StatePlayerSelectTarget();
            FSMState stateAI = new StateAI();

            FSMTransition transitionWaitToSelect = new TransitionWaitToSelect(statePlayerSelected);
            stateWaitPlayerSelect.Transitions.Add(transitionWaitToSelect);
            states.Add(stateWaitPlayerSelect);

            FSMTransition transitionSelectToSelect = new TransitionSelectToSelect(statePlayerSelected);
            FSMTransition transitionSelectToSelectAction = new TransitionSelectToSelectAction(statePlayerSelectAction);
            FSMTransition transitionEndTurnToAI = new TransitionEndTurnToAI(stateAI);
            statePlayerSelected.Transitions.Add(transitionSelectToSelect);
            statePlayerSelected.Transitions.Add(transitionSelectToSelectAction);
            statePlayerSelected.Transitions.Add(transitionEndTurnToAI);
            states.Add(statePlayerSelected);

            FSMTransition transitionSelectActionToSelectTarget = new TransitionSelectActionToSelectTarget(statePlayerSelectTarget);
            statePlayerSelectAction.Transitions.Add(transitionSelectActionToSelectTarget);
            statePlayerSelectAction.Transitions.Add(transitionEndTurnToAI);
            states.Add(statePlayerSelectAction);

            FSMTransition transitionSelectTargetToSelectAction = new TransitionSelectTargetToSelectAction(statePlayerSelectAction);
            FSMTransition transitionSelectTargetToAI = new TransitionSelectTargetToAI(stateAI);
            FSMTransition transitionSelectTargetToSelectTarget = new TransitionSelectTargetToSelectTarget(statePlayerSelectTarget);
            statePlayerSelectTarget.Transitions.Add(transitionSelectTargetToSelectAction);
            statePlayerSelectTarget.Transitions.Add(transitionSelectTargetToAI);
            statePlayerSelectTarget.Transitions.Add(transitionSelectTargetToSelectTarget);
            statePlayerSelectTarget.Transitions.Add(transitionEndTurnToAI);
            states.Add(statePlayerSelectTarget);

            FSMTransition transitionAIToWaitPlayer = new TransitionAIToWaitPlayer(stateWaitPlayerSelect);
            FSMTransition transitionAIToAI = new TransitionAIToAI(stateAI);
            stateAI.Transitions.Add(transitionAIToWaitPlayer);
            stateAI.Transitions.Add(transitionAIToAI);
            states.Add(stateAI);

            initState = stateWaitPlayerSelect;
            activeState = stateWaitPlayerSelect;
            Bootstrap.getInput().addListener(this);
        }

        public void update()
        {
            
            foreach (FSMTransition transition in activeState.Transitions)
            {
                if(transition.IsValid)
                {
                    transition.IsValid = false;
                    activeState.onExit();
                    activeState = transition.NextState;
                    activeState.onEnter();
                }
            }
            activeState.onUpdate();
        }

        public void handleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseDown" && inp.Button == 1)
            {
                activeState.handleInput(inp, eventType);
            }
        }
    }
}

