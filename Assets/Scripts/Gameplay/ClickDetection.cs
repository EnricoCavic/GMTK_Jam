using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPA.Managers;
using System;
using UnityEngine.InputSystem;

public class ClickDetection : MonoBehaviour
{
    private Camera mainCam;
    private PlayerActions actions;

    private RaycastHit2D tempHit;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        actions = GetComponent<InputManager>().actions;
        actions.Gameplay.Select.performed += OnClickPerformed;
        actions.Gameplay.Select.canceled += OnClickCanceled;
    }

    private void OnClickPerformed(InputAction.CallbackContext obj)
    {
        tempHit = Physics2D.GetRayIntersection(mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!tempHit.collider) return;

        Debug.Log(tempHit.collider.gameObject.name);
    }

    private void OnClickCanceled(InputAction.CallbackContext obj)
    {
        tempHit = Physics2D.GetRayIntersection(mainCam.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!tempHit.collider) return;

        Debug.Log(tempHit.collider.gameObject.name);
    }

    private void OnDisable()
    {
        actions.Gameplay.Select.performed -= OnClickPerformed;
        actions.Gameplay.Select.canceled -= OnClickCanceled;
    }
}
