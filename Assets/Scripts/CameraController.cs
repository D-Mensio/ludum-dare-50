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

    private bool converging;
    private Vector3 convergingPos;
    private float progress;
    private Vector3 startingPos;

    void Update()
    {
        if (!converging)
        {
            downLimit += expandRate * Time.deltaTime;
            horizontalLimit += expandRate * Time.deltaTime / 2f;

            float verticalMovement = Input.GetAxisRaw("Vertical") * speed * Time.unscaledDeltaTime;
            float horizontalMovement = Input.GetAxisRaw("Horizontal") * speed * Time.unscaledDeltaTime;

            Vector3 pos = transform.localPosition;
            float verticalPos = Mathf.Clamp(pos.y + verticalMovement, -downLimit, 0);
            float horizontalPos = Mathf.Clamp(pos.x + horizontalMovement, -horizontalLimit, horizontalLimit);

            transform.localPosition = new Vector3(horizontalPos, verticalPos, pos.z);
        }
        else
        {
            progress += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(startingPos, convergingPos, progress);
            cam.orthographicSize = Mathf.Lerp(4, 2, progress);
        }
    }

    public void Converge(Vector3 pos)
    {
        converging = true;
        convergingPos = new Vector3(pos.x, pos.y, transform.position.z);
        startingPos = transform.position;
    }


}
