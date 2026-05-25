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
        Vector3 velocity = playerRb.velocity;

        velocity.y = 0;

        // only update direction if moving enough
        if (velocity.sqrMagnitude > 0.1f)
        {
            Vector3 targetDirection = -velocity.normalized;

            currentDirection = Vector3.Slerp(
                currentDirection,
                targetDirection,
                rotationSmoothness * Time.deltaTime
            );
        }
        

        Vector3 desiredPosition =
            player.position +
            currentDirection * distance +
            Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            positionSmoothness * Time.deltaTime
        );

        transform.LookAt(player);
    }
}