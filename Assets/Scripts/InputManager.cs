using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Parameters")]
    private bool paused = false;
    private bool doubleSpeed = false;
    private bool gameOver;
    [SerializeField]
    private LayerMask bucketLayerMask;
    private bool holding;
    [SerializeField]
    private float swipeDetectTime;
    private float currentDetectTime;
    private Vector2 originalPos;
    [SerializeField] 
    private float swipeThreshold;
   
    [Header("Object Connections")]
    [SerializeField]
    private ConnectionManager cm;
    [SerializeField]
    private BucketManager bm;
    [SerializeField]
    private CameraController cc;
    [SerializeField]
    private Toggle pauseButton;
    [SerializeField]
    private Toggle playButton;
    [SerializeField]
    private Toggle doubleSpeedButton;
    public void Update()
    {
        if (!gameOver)
        {
            //Pause/Unpause
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (paused)
                    TogglePlay();
                else
                    TogglePause();
            }

            ProcessInputs();
        }
    }

    private void ProcessInputs()
    {
        if (Input.GetMouseButtonDown(0) && !PointerOverUI())
        {
            if (!holding)
            {
                holding = true;
                currentDetectTime = swipeDetectTime;
                originalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else if (holding && Input.GetMouseButton(0))
        {
            currentDetectTime -= Time.unscaledDeltaTime;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(originalPos, bucketLayerMask);

            if (hitCollider)
            {
                Bucket hitBucket = hitCollider.gameObject.GetComponent<Bucket>();
                if (hitBucket.TryGetClosestConnectionPoint(out ConnectionPoint startingPoint, mousePos))
                {
                    holding = false;
                    cm.StartPlacingConnection(startingPoint);
                }
            }
            else if ((mousePos - originalPos).magnitude > swipeThreshold)
            {
                holding = false;
                cc.moving = true;
            }
            else if (currentDetectTime <= 0)
            {
                holding = false;
                bm.StartPlacingBucket();
            }
        }
        else if (holding && Input.GetMouseButtonUp(0))
        {
            holding = false;
            bm.StartPlacingBucket();
        }

    }

    public void Pause()
    {
        if (!gameOver)
        {
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void TogglePause()
    {
        pauseButton.isOn = true;
    }

    public void SetNormalSpeed()
    {
        if (!gameOver)
        {
            doubleSpeed = false;
            paused = false;
            Time.timeScale = 1f;
        }
    }

    public void SetDoubleSpeed()
    {
        if (!gameOver)
        {
            doubleSpeed = true;
            paused = false;
            Time.timeScale = 2f;
        }
    }

    public void TogglePlay()
    {
        if (doubleSpeed)
        {
            doubleSpeedButton.isOn = true;
        }
        else
        {
            playButton.isOn = true;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        bm.StopPlacingBucket();
        cm.StopPlacingConnection();
    }

    private bool PointerOverUI()
    {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
        }

}
