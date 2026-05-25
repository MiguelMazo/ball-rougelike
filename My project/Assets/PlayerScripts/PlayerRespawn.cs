using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public float fallThreshold = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // reset position
        transform.position = startPosition;
        transform.rotation = startRotation;

        // IMPORTANT: clear movement so it doesn't keep flying
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}