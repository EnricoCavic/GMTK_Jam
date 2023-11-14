using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DPA.Managers;
using UnityEngine.InputSystem;

public class VolumeSwitcher : MonoBehaviour
{
    Volume volume;

    public VolumeProfile resumedProfile;
    public VolumeProfile pausedProfile;
    
    VolumeProfile selectedProfile;
    

    void Awake()
    {
        volume = this.GetComponent<Volume>();
        selectedProfile = resumedProfile;
    }

    void Start()
    {
        InputManager.Instance.actions.Gameplay.SwitchResumePause.performed += SwitchVolume;
    }


    public void SwitchVolume(InputAction.CallbackContext context)
    {
        Debug.Log("troca");
        selectedProfile = (selectedProfile == resumedProfile ? pausedProfile : resumedProfile);
        volume.profile = selectedProfile;
    }
}
