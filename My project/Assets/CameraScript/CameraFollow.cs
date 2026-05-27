using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 6f;
    public float height = 4f;
    public float lookTargetVariable = 1f;

    public float rotationSmoothness = 3f;
    public float positionSmoothness = 5f;

    private Rigidbody playerRb;

    private Vector3 currentDirection;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();

        currentDirection = -player.forward;
    }

   void LateUpdate()
{
   // Get player velocity from physics
    Vector3 velocity = playerRb.velocity;

    // Ignore vertical movement so camera doesn't react to jumping/falling
    velocity.y = 0;

    float speed = velocity.magnitude;

    // Default target direction is "keep current direction"
    // This prevents jitter when the player is barely moving
    Vector3 targetDirection = currentDirection;

    // Only update camera direction if movement is meaningful
    if (speed > 0.1f)
    {
        Vector3 camForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

        Vector3 localVelocity =
            camForward * Vector3.Dot(velocity, camForward) +
            camRight * Vector3.Dot(velocity, camRight);

        targetDirection = -localVelocity.normalized;
    }

    // Convert speed into 0–1 range so we can scale camera responsiveness
    float speedFactor = Mathf.Clamp01(speed / 20f);

    // Rotation speed increases as player moves faster
    float dynamicRotationSpeed = Mathf.Lerp(0.3f, 3f, speedFactor);

    // Smoothly rotate camera direction toward target direction
    // Time.deltaTime ensures frame-rate independent smoothing
    currentDirection = Vector3.Slerp(
        currentDirection,
        targetDirection,
        dynamicRotationSpeed * Time.deltaTime
    );

    // Compute final camera position around player
    Vector3 followDir = currentDirection;
    followDir.y = -0.05f;
    followDir.Normalize();

    Vector3 desiredPosition =
        player.position +
        followDir * distance +
        Vector3.up * height;

// Press P to snap camera behind the player, useful if you get disoriented or want to quickly look forward.
    if (Input.GetKeyDown(KeyCode.P))
        {
            currentDirection = -player.forward;
        }   
    // Smooth camera position movement (prevents snapping)
    transform.position = Vector3.Lerp(
        transform.position,
        desiredPosition,
        positionSmoothness * Time.deltaTime
    );

    // Always look at above the player, so they stay in the bottom 3rd of the screen. 
    Vector3 lookTarget = player.position + Vector3.up * lookTargetVariable;
    transform.LookAt(lookTarget);
}
}