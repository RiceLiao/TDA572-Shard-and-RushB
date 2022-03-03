/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectToSelect : FSMTransition
    {
        public TransitionSelectToSelect(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

