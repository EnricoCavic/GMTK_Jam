using DPA.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

// script vai ter que ser refatorado para aceitar clicks do mouse e touchscreen
public class MovableObject : MonoBehaviour
{
    private Camera mainCam;
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    public bool isSelected = false;
    private Vector2 clickOffset = new(0,0);

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
        resumePause.onResumed += DeselectObj;
    }

    private void Update()
    {
        if (!resumePause.isPaused) return;
        if (!isSelected) return;

        Vector2 moveDirection = GetMousePosition() + clickOffset - (Vector2)transform.position;
        rb.velocity = moveDirection * moveSpeed;
    }

    // refatorar para usar o input system
    private void OnMouseDown()
    {
        if (!resumePause.isPaused) return;
        SelectObj();
    }

    private void OnMouseUp()
    {
        if (!resumePause.isPaused) return;
        DeselectObj();
    }

    private void SelectObj()
    {
        clickOffset = (Vector2)transform.position - GetMousePosition();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.bodyType = RigidbodyType2D.Dynamic;
        isSelected = true;
    }

    private void DeselectObj()
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
        resumePause.onResumed -= DeselectObj;
    }

}
