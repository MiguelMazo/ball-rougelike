using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cameraTransform;

    // Multiplies Unity gravity for stronger or weaker falling
    public float gravityMultiplier = 1f;
    public float scaler = 0.1f;

    public float groundForce = 17f;
    public float airForce = 0f;

    public float groundCheckDistance = 1.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.inertiaTensorRotation = new Quaternion(0.01f, 0.01f, 0.01f, 1f);

        // Apply damping torque opposite to angular velocity
        rb.AddTorque(-rb.angularVelocity * scaler, ForceMode.Acceleration);
        // Read player input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Clamp input so diagonal movement is not faster than cardinal movement
        Vector3 input = new Vector3(h, 0f, v);
        input = Vector3.ClampMagnitude(input, 1f);

        // Get camera-relative directions on the horizontal plane
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Convert input into world-space movement direction relative to camera
        Vector3 moveDir =
            camForward * input.z +
            camRight * input.x;

        // Desired base movement force applied to the ball
        bool isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance
        );

        float moveForce;

        if (isGrounded)
        {
            moveForce = groundForce;
        }
        else
        {
            moveForce = airForce;
        }

        // Extract horizontal velocity (ignore vertical component)
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        // Current horizontal speed
        float speed = horizontalVelocity.magnitude;

        // Normalize speed into 0–1 range based on an expected max speed of 20
        float speed01 = Mathf.Clamp01(speed / 20f);

        // Acceleration multiplier:
        // - lower at low speed for smoother start
        // - higher at higher speed for stronger sustained movement
        float accelMultiplier = Mathf.Lerp(0.4f, 1.2f, speed01 * speed01);

        // Apply movement force with speed-based scaling
        rb.AddForce(
            moveDir * moveForce * accelMultiplier,
            ForceMode.Acceleration
        );

        // Apply modified gravity for consistent falling behavior
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        float maxSpeed = 30f;

        if (speed > maxSpeed)
        {
            Vector3 limited = horizontalVelocity.normalized * maxSpeed;

            rb.velocity = new Vector3(
                limited.x,
                rb.velocity.y,
                limited.z
            );
        }
        
    }

    
}