using DPA.Generic;

namespace DPA.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public PlayerActions inputActions;

        private void Awake()
        {
            if (!InstanceSetup(this)) return;

            inputActions = new();
            inputActions.Enable();
        }
    }
}