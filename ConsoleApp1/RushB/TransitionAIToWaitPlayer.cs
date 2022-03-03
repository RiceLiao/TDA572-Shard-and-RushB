/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionAIToWaitPlayer : FSMTransition
    {
        public TransitionAIToWaitPlayer(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

