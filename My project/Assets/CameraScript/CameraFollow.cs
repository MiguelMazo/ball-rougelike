using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 6f;
    public float height = 4f;

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
        // Convert velocity into a direction (where the player is actually moving)
        targetDirection = -velocity.normalized;
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
    Vector3 desiredPosition =
        player.position +
        currentDirection * distance +
        Vector3.up * height;

    // Smooth camera position movement (prevents snapping)
    transform.position = Vector3.Lerp(
        transform.position,
        desiredPosition,
        positionSmoothness * Time.deltaTime
    );

    // Always look at player so they stay centered in view
    transform.LookAt(player);
}
}