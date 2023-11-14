using DPA.Managers;
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
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.bodyType = RigidbodyType2D.Static;
        resumePause = ResumePauseHandler.Instance;
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
        rb.velocity = Vector2.zero;

        Vector2 currentPos = rb.transform.position;
        rb.position = new(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y));

        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.bodyType = RigidbodyType2D.Static;
        isSelected = false;
    }

    private Vector2 GetMousePosition()
    {
        return mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

}
