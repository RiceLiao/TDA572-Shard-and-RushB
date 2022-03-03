/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/
using Shard;
using System.Collections.Generic;

namespace RushB
{
    abstract class FSMState
    {
        public abstract void onEnter();
        public abstract void onUpdate();
        public abstract void onExit();
        private List<FSMTransition> transitions;

        public List<FSMTransition> Transitions { get => transitions; set => transitions = value; }

        public abstract void handleInput(InputEvent inp, string eventType);
    }
}

