/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectTargetToSelectAction : FSMTransition
    {
        public TransitionSelectTargetToSelectAction(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

