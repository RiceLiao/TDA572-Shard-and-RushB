/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

namespace RushB
{
    abstract class FSMTransition
    {
        private bool isValid;
        private FSMState nextState;

        public bool IsValid { get => isValid; set => isValid = value; }
        public FSMState NextState { get => nextState; set => nextState = value; }

        public abstract void onTransition();
    }
}

