using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceBucketSpawner : MonoBehaviour
{
    public GameObject sourceBucketPrefab;

    public CameraController cc;

    public ConnectionPlacer cp;

    public float spawnDelay;

    private float lastSpawnTime;

    public LayerMask bucketLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastSpawnTime + spawnDelay)
        {
            lastSpawnTime = Time.time;
            SpawnSourceBucket(GetRandomPos());
        }
    }

    private void SpawnSourceBucket(Vector3 pos)
    {
        Bucket source = Instantiate(sourceBucketPrefab, pos, Quaternion.identity).GetComponentInChildren<Bucket>();
        cp.connectionsPoints.Add(source.connectionPoint1);
        cp.connectionsPoints.Add(source.connectionPoint2);
    }

    private Vector3 GetRandomPos()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize;
        float width = height * cam.aspect;
        var horizontalRange = new Vector2(-cc.horizontalLimit - width + 1, cc.horizontalLimit + width - 1);
        var verticalRange = new Vector2(-cc.downLimit/2 + 1f, height - 1);
        Vector3 pos = new Vector3(Random.Range(horizontalRange.x, horizontalRange.y), Random.Range(verticalRange.x, verticalRange.y), 0);
        if (!Physics2D.OverlapBox(pos, new Vector2(1.5f, 1.5f), 0, bucketLayerMask))
        {
            return pos;
        }
        else
            return GetRandomPos();
    }
}
