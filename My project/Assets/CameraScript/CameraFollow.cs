using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 6f;
    public float height = 4f;
    public float lookTargetVariable = 1f;

    public float rotationSmoothness = 3f;
    public float positionSmoothness = 5f;

    public float maxRollAngle = 5f;
    public float maxPitchAngle = 3f;
    public float cameraTiltSmoothness = 6f;

    public float forwardDistanceBoost = 2f;
    public float backwardHeightBoost = 1.5f;

    public float cameraResponseSpeed = 5f;

    private float currentRoll;
    private float currentPitch;

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

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

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

    // forward = push camera back
    float dynamicDistance =
        distance + (verticalInput * forwardDistanceBoost);

    // backward = raise camera slightly
    float dynamicHeight =
        height + (-verticalInput * backwardHeightBoost);

    Vector3 desiredPosition =
        player.position +
        followDir * dynamicDistance +
    Vector3.up * dynamicHeight;

    // Press P to snap camera behind the player, useful if you get disoriented or want to quickly look forward. 
        if (Input.GetKeyDown(KeyCode.P))
            {
                Vector3 vel = playerRb.velocity;
                vel.y = 0f;

                if (vel.sqrMagnitude > 0.01f)
                {
                    currentDirection = -vel.normalized;
                }
            }   
        // Smooth camera position movement (prevents snapping)
        transform.position = Vector3.Lerp(
        transform.position,
        desiredPosition,
        positionSmoothness * Time.deltaTime
    );

    Vector3 lookTarget = player.position + Vector3.up * lookTargetVariable;
    transform.LookAt(lookTarget);

    // target tilt
    float targetRoll = -horizontalInput * maxRollAngle * speedFactor;
    float targetPitch = verticalInput * maxPitchAngle * speedFactor;

    // smooth tilt
    currentRoll = Mathf.Lerp(
        currentRoll,
        targetRoll,
        cameraTiltSmoothness * Time.deltaTime
    );

    currentPitch = Mathf.Lerp(
        currentPitch,
        targetPitch,
        cameraTiltSmoothness * Time.deltaTime
    );

    // apply tilt AFTER LookAt (this is the key difference)
    transform.Rotate(
        currentPitch,
        0f,
        currentRoll,
        Space.Self
    );
    }
}