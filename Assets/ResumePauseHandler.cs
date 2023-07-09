using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPA.Generic;
using UnityEngine.InputSystem;

namespace DPA.Managers
{
    public class ResumePauseHandler : Singleton<ResumePauseHandler>
    {
        InputManager inputManager;
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

        void SwitchState(InputAction.CallbackContext callbackContext)
        {
            pauseBot = !pauseBot;
        }


    }

}