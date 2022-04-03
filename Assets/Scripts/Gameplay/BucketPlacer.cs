using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketPlacer : MonoBehaviour
{
    public ConnectionPlacer cp;

    public int bucketsAvailable;

    public GameObject bucketPrefab;

    public bool placeable;

    public LayerMask bucketLayerMask;

    public SpriteRenderer posIndicator;

    public bool placingBucket;

    public float refillTime;
    float lastRefill;

    public TMPro.TextMeshProUGUI bucketNumberIndicator;
    public Image fillImage;

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

            if (placeable && Input.GetMouseButtonDown(0))
            {
                PlaceBucket(mousePos);
            }
        }
    }

    private void PlaceBucket(Vector3 position)
    {
        Bucket bucket = Instantiate(bucketPrefab, position, Quaternion.identity).GetComponent<Bucket>();
        cp.connectionsPoints.Add(bucket.connectionPoint1);
        cp.connectionsPoints.Add(bucket.connectionPoint2);       
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
        if (!placingBucket)
        {
            if (bucketsAvailable > 0)
            {
                placingBucket = true;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector3(mousePos.x, mousePos.y, -5);
                posIndicator.transform.localPosition = mousePos;
                posIndicator.transform.localScale = Vector3.one;
            }
            else
                Debug.Log("Not enough buckets!");
        }
        else
            Debug.Log("Already placing bucket!");
    }

}
