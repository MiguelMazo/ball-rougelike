using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 6f;
    public float minDistance = 3f;
    public float maxDistance = 10f;

    public float height = 2f;

    public float mouseSensitivity = 3f;
    public float zoomSensitivity = 10f;

    private float yaw;

     void Start()
    {
        // Lock yaw to player's facing direction at start
        yaw = player.eulerAngles.y;
    }

    void LateUpdate()
    {
        // 🧭 rotate camera around player
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

        // 🔍 zoom in/out
        float scroll = Input.GetAxis("Mouse Y");
        distance -= scroll * zoomSensitivity;

        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // direction around player
        Quaternion rotation = Quaternion.Euler(0, yaw, 0);

        Vector3 direction = rotation * Vector3.back;

        // 👇 KEY CHANGE: low ground-follow position
        Vector3 targetPosition =
            player.position
            + Vector3.up * height
            + direction * distance;

        transform.position = targetPosition;

        // always look at player slightly above center
        transform.LookAt(player.position + Vector3.up * 1f);
    }
}