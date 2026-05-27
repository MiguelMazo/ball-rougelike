using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody playerRb;
    public TextMeshProUGUI speedText;

    void Update()
    {
        Vector3 flatVelocity = playerRb.velocity;
        flatVelocity.y = 0;

        float speed = playerRb.velocity.magnitude;

        speedText.text = "Speed: " + speed.ToString("0.0");
        if (speed < 5)
        speedText.color = Color.white;
        else if (speed < 10)
            speedText.color = Color.yellow;
        else
            speedText.color = Color.red;

        Debug.Log($"Speed: {playerRb.velocity.magnitude} | Flat: {flatVelocity.magnitude}");
    }
}