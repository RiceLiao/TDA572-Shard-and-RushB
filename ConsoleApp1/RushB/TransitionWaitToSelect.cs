/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionWaitToSelect : FSMTransition
    {
        public TransitionWaitToSelect(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

