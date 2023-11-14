using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPA.Generic;
using UnityEngine.InputSystem;
using System;

namespace DPA.Managers
{
    public class ResumePauseHandler : Singleton<ResumePauseHandler>
    {
        public InputManager inputManager;
        public Action onPaused;
        public Action onResumed;
        public bool isPaused = false;

        void Awake()
        {
            if (!InstanceSetup(this)) return;
        }

        void Start()
        {
            inputManager = InputManager.Instance;
            inputManager.actions.Gameplay.SwitchResumePause.performed += SwitchState;
        }

        public void Restart() => isPaused = false;

        void SwitchState(InputAction.CallbackContext callbackContext)
        {
            isPaused = !isPaused;
            if(isPaused) onPaused?.Invoke();
            else onResumed?.Invoke();
        }
    }

}