/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectTargetToAI : FSMTransition
    {
        public TransitionSelectTargetToAI(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

