using UnityEngine;
using UnityEngine.UI;

public class BucketManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private int bucketsAvailable;
    [SerializeField]
    private GameObject bucketPrefab;
    [SerializeField]
    private LayerMask bucketLayerMask;
    [SerializeField]
    private SpriteRenderer posIndicator;
    private bool placeable;
    private bool placingBucket;
    [SerializeField]
    private float refillTime;
    private float lastRefill;

    [Header("Object Connections")]
    [SerializeField]
    private TMPro.TextMeshProUGUI bucketNumberIndicator;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Animator bucketResourceAC;

    void Update()
    {
        //refill
        bucketNumberIndicator.text = bucketsAvailable.ToString();
        float progress = (Time.time - lastRefill) / refillTime;
        fillImage.fillAmount = progress;
        if (progress >= 1)
        {
            lastRefill = Time.time;
            bucketsAvailable++;
        }

        //place bucket
        if (placingBucket)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, -5);
            posIndicator.transform.localPosition = mousePos;
            if (Physics2D.OverlapBox(mousePos, new Vector2(1, 1), 0, bucketLayerMask))
            {
                placeable = false;
                posIndicator.color = Color.red;
            }
            else
            {
                placeable = true;
                posIndicator.color = Color.white;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (placeable)
                    PlaceBucket(mousePos);
                else
                    StopPlacingBucket();
            }
        }
    }

    private void PlaceBucket(Vector3 position)
    {
        Instantiate(bucketPrefab, position, Quaternion.identity);
        bucketsAvailable--;
        StopPlacingBucket();
    }

    public void StopPlacingBucket()
    {
        if (placingBucket)
        {
            placingBucket = false;
            posIndicator.transform.localScale = Vector3.zero;
        }
    }

    public void StartPlacingBucket()
    {
        if (!placingBucket && bucketsAvailable > 0)
        {
            placingBucket = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, -5);
            posIndicator.transform.localPosition = mousePos;
            posIndicator.transform.localScale = Vector3.one;
        }
        else
        {
            bucketResourceAC.SetTrigger("FlashRed");
        }
    }
}
