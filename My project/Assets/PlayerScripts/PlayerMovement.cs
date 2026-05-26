using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float force = 20f;

    public Transform cameraTransform;
    public float gravityMultiplier = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 movement =
            cameraForward * vertical +
            cameraRight * horizontal;
        
        rb.AddForce(movement * force);
        rb.AddForce(
            Physics.gravity * gravityMultiplier,
            ForceMode.Acceleration
        );
    }
}