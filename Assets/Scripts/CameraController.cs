using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public float expandRate;
    [NonSerialized]
    public float downLimit;
    [NonSerialized]
    public float horizontalLimit;
    [SerializeField]
    private float speed;

    public bool moving = false;
    private bool converging;
    private Vector3 startingPos;
    private Vector3 convergingPos;
    private float convergingProgress;   
    
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private SpriteRenderer borderVisualizer;

    private void Awake()
    {
        #if UNITY_ANDROID
            speed = speed / 5;
        #endif
    }

    void Update()
    {
        if (!converging)
        {
            downLimit += expandRate * Time.deltaTime;
            horizontalLimit += expandRate * Time.deltaTime / 2f;

            var height = 2 * cam.orthographicSize + downLimit;
            var width = (cam.orthographicSize * Camera.main.aspect + horizontalLimit) * 2;

            borderVisualizer.size = new Vector2(width, height);
            borderVisualizer.transform.position = new Vector3(0, -downLimit/2f, 5);
            MoveCamera();
        }
        else
        {
            convergingProgress += Time.unscaledDeltaTime;
            transform.localPosition = Vector3.Lerp(startingPos, convergingPos, convergingProgress);
            cam.orthographicSize = Mathf.Lerp(4, 2, convergingProgress);
        }
    }

    private void MoveCamera()
    {
        if (moving)
        {
            float verticalMovement = -Input.GetAxisRaw("Mouse Y") * speed * Time.unscaledDeltaTime;
            float horizontalMovement = -Input.GetAxisRaw("Mouse X") * speed * Time.unscaledDeltaTime;

            Vector3 pos = transform.localPosition;
            float verticalPos = Mathf.Clamp(pos.y + verticalMovement, -downLimit, 0);
            float horizontalPos = Mathf.Clamp(pos.x + horizontalMovement, -horizontalLimit, horizontalLimit);

            transform.localPosition = new Vector3(horizontalPos, verticalPos, pos.z);

            if (Input.GetMouseButtonUp(0))
                moving = false;
        }
    }

    public void Converge(Vector3 pos)
    {
        converging = true;
        convergingPos = new Vector3(pos.x, pos.y, transform.position.z);
        startingPos = transform.position;
        borderVisualizer.gameObject.SetActive(false);
    }


}
