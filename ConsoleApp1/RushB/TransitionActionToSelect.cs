/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    class TransitionActionToSelect : FSMTransition
    {
        public TransitionActionToSelect(FSMState state)
        {
            NextState = state;
        }
        public override void onTransition()
        {

        }
    }
}

