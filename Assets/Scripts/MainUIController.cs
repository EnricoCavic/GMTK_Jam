using DPA.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    private PlayerActions pActions;

    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject pauseUI;

    private void OnEnable()
    {
        pActions = InputManager.Instance.actions;
        pActions.Gameplay.SwitchResumePause.performed += SwitchResumePause;
    }

    private void OnDisable()
    {
        pActions.Gameplay.SwitchResumePause.performed -= SwitchResumePause;
    }

    private void SwitchResumePause(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playUI.SetActive(!playUI.activeInHierarchy);
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
    }
}
