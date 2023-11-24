using System;
using DPA.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovableObject : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;

    public bool isSelected = false;

    ResumePauseHandler resumePause;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        mainCam = Camera.main;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.bodyType = RigidbodyType2D.Static;
        resumePause = ResumePauseHandler.Instance;
        resumePause.onResumed += ResumeObj;
    }

    private void Update()
    {
        if (!resumePause.isPaused) return;
        if (!isSelected) return;

        Vector2 move = GetMousePosition() - (Vector2)transform.position;
        rb.velocity = move * moveSpeed;
    }

    private void OnMouseDown()
    {
        if (!resumePause.isPaused) return;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isSelected = true;
    }

    private void OnMouseUp()
    {
        if (!resumePause.isPaused) return;
        ResumeObj();
    }

    private void ResumeObj()
    {        
        if (!isSelected) return;
        rb.velocity = Vector2.zero;

        Vector2 currentPos = transform.position;
        // esse round precisa ser para números terminados em .5
        transform.position = new(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y));

        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.bodyType = RigidbodyType2D.Static;
        isSelected = false;
    }

    private Vector2 GetMousePosition()
    {
        return mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void OnDestroy()
    {
        resumePause.onResumed -= ResumeObj;
    }

}
