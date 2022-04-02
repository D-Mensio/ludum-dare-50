using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;

    public float expandRate;

    public float downLimit;

    public float horizontalLimit;

    public float speed;

    void Update()
    {
        downLimit += expandRate * Time.deltaTime;
        horizontalLimit += expandRate * Time.deltaTime / 2f;

        float verticalMovement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float horizontalMovement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector3 pos = transform.localPosition;
        float verticalPos = Mathf.Clamp(pos.y + verticalMovement, -downLimit, 0);
        float horizontalPos = Mathf.Clamp(pos.x + horizontalMovement, -horizontalLimit, horizontalLimit);

        transform.localPosition = new Vector3(horizontalPos, verticalPos, pos.z);
    }
}
