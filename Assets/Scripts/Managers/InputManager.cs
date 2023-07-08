using DPA.Generic;

namespace DPA.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerActions actions;

        private void Awake()
        {
            if (!InstanceSetup(this)) return;

            actions = new();
            actions.Enable();
        }
    }
}