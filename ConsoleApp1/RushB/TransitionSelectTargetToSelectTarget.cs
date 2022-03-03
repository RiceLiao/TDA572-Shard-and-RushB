/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectTargetToSelectTarget : FSMTransition
    {
        public TransitionSelectTargetToSelectTarget(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

