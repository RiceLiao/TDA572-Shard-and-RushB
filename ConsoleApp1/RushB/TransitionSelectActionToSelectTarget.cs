/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectActionToSelectTarget : FSMTransition
    {
        public TransitionSelectActionToSelectTarget(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

