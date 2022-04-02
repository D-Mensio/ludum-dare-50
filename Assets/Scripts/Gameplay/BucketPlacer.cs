using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPlacer : MonoBehaviour
{
    public ConnectionPlacer cp;

    public int bucketsAvailable;

    public GameObject bucketPrefab;

    public bool placeable;

    public LayerMask bucketLayerMask;

    public SpriteRenderer posIndicator;

    public bool placingBucket;

    void Update()
    {
        if (placingBucket)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            posIndicator.transform.localPosition = mousePos - new Vector3(0, 0, 5);
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
                posIndicator.transform.localScale = Vector3.one;
            }
            else
                Debug.Log("Not enough buckets!");
        }
        else
            Debug.Log("Already placing bucket!");
    }

}
