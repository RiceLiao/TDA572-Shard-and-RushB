/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionEndTurnToAI : FSMTransition
    {
        public TransitionEndTurnToAI(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

