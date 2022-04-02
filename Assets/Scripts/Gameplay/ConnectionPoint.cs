using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public bool isLeft;

    public Bucket ownBucket;

    public Bucket connectedBucket;

    public bool connected;

    public float initialCapacity;

    public float capacity;

    private void Update()
    {
        
        capacity = initialCapacity * Time.deltaTime;
    }

    public bool ValidateConnection(Vector3 otherPosition)
    {
        Vector3 ownPosition = transform.position;

        //Vector3 connectPosition = otherBucket.GetConnectPosition();
        return (otherPosition.y < ownPosition.y && ((isLeft && otherPosition.x < ownPosition.x) || (!isLeft && otherPosition.x > ownPosition.x)));
    }

    public void CreateConnection(Bucket bucket)
    {
        connected = true;
        connectedBucket = bucket;
    }

    public float SendWater(float water)
    {
        float rejectedWater = connectedBucket.ReceiveWater(water);
        if (rejectedWater > 0)
            capacity = 0;
        else
            capacity -= water;
        return rejectedWater;
    }

    public bool IsOpen()
    {
        return connected && capacity > 0;
    }

}
