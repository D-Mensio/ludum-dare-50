using System;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    [SerializeField]
    private bool isLeft;
    private Bucket connectedBucket;
    [NonSerialized]
    public bool isConnected;
    [SerializeField]
    private float capacityPerSecond;
    [NonSerialized]
    public float currentCapacity;

    private void Update()
    {
        currentCapacity = capacityPerSecond * Time.deltaTime;
    }

    public bool ValidateConnection(Vector3 otherPosition)
    {
        Vector3 ownPosition = transform.position;
        return (otherPosition.y < ownPosition.y && ((isLeft && otherPosition.x < ownPosition.x) || (!isLeft && otherPosition.x > ownPosition.x)));
    }

    public void CreateConnection(Bucket bucket)
    {
        isConnected = true;
        connectedBucket = bucket;
    }

    public float SendWater(float water)
    {
        float rejectedWater = connectedBucket.ReceiveWater(water);
        if (rejectedWater > 0)
            currentCapacity = 0;
        else
            currentCapacity -= water;
        return rejectedWater;
    }

    public bool IsOpen()
    {
        return isConnected && currentCapacity > 0;
    }

}
