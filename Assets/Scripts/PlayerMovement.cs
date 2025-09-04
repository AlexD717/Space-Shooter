using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrust;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationThrust;
    [SerializeField] private float maxRotationSpeed;

    [Header("References")]
    [SerializeField] private InputActionAsset inputActions;
    
    private InputAction move;
    private Rigidbody2D rb;

    private void OnEnable()
    {
        InputActionMap playerControls = inputActions.FindActionMap("Player");
        move = playerControls.FindAction("Move");
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = move.ReadValue<Vector2>();

        HandleThrust(moveInput.y);
        HandleRotation(moveInput.x);
    }

    private void HandleThrust(float acceleration)
    {
        if (acceleration > 0)
        {
            // Accelerate
            Vector2 force = transform.up * acceleration * thrust;
            rb.AddForce(force);
        }
        else if (acceleration < -0.1f)
        {
            // Brake
            Vector2 oppositeForce = -rb.linearVelocity.normalized * thrust;
            rb.AddForce(oppositeForce);

            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        // Clamp max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    private void HandleRotation(float rotation)
    {
        if (Mathf.Abs(rotation) < 0.1)
            return;

        float targetRotationSpeed = -rotation * maxRotationSpeed;
        rb.angularVelocity = Mathf.MoveTowards(rb.angularVelocity, targetRotationSpeed, rotationThrust * Time.fixedDeltaTime);
    }
}
