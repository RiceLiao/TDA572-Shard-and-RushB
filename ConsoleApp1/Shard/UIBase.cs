/*
*
*   The Base of UI, any UI elements should extend from GameObject.
*   @author Haowei Liao
*   @version 1.0
*   
*/


namespace Shard
{

    abstract class UIBase : GameObject, InputListener
    {
        private bool attaching;
        private bool active;

        public bool Attaching { get => attaching; set => attaching = value; }
        public bool Active { get => active; set => active = value; }

        public UIBase()
        {
            this.Visible = true;
            this.Attaching = false;
            this.Active = true;
            Bootstrap.getInput().addListener(this);
        }

        public abstract void handleInput(InputEvent inp, string eventType);

    }
}
