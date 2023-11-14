using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPA.Generic;
using UnityEngine.InputSystem;

namespace DPA.Managers
{
    public class ResumePauseHandler : Singleton<ResumePauseHandler>
    {
        public InputManager inputManager;
        public bool pauseBot = false;

        void Awake()
        {
            if (!InstanceSetup(this)) return;
        }

        void Start()
        {
            inputManager = InputManager.Instance;
            inputManager.actions.Gameplay.SwitchResumePause.performed += SwitchState;
        }

        public void Restart() => pauseBot = false;

        void SwitchState(InputAction.CallbackContext callbackContext) => pauseBot = !pauseBot;


    }

}