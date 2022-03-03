/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionAIToAI : FSMTransition
    {
        public TransitionAIToAI(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

