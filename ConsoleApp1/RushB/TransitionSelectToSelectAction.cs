/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionSelectToSelectAction : FSMTransition
    {
        public TransitionSelectToSelectAction(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

