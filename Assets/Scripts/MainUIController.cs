using DPA.Managers;
using System.Collections;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private GameObject playUI;
    [SerializeField] private GameObject pauseUI;

    private void OnEnable()
    {
        IEnumerator TryGetManager()
        {
            while(inputManager == null)
            {
                inputManager = InputManager.Instance;
                yield return null;
            }
            inputManager.actions.Gameplay.SwitchResumePause.performed += SwitchResumePause;
        }
        StartCoroutine(TryGetManager());
    }

    private void OnDisable()
    {
        inputManager.actions.Gameplay.SwitchResumePause.performed -= SwitchResumePause;
    }

    

    private void SwitchResumePause(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playUI.SetActive(!playUI.activeInHierarchy);
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
    }
}
