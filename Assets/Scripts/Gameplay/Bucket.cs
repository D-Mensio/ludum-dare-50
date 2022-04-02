using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public ConnectionPoint connectionPoint1, connectionPoint2;

    public List<ConnectionPoint> parents;

    public float waterLevel = 0;

    public GameObject water;

    void LateUpdate()
    {
        water.transform.localScale = new Vector3(1, waterLevel, 1);
    }

    public float ReceiveWater(float amount)
    {
        float rejectedWater = 0;
        waterLevel += amount;
        if (waterLevel > 0.5f)
            SendWaterToChildren();
        if (waterLevel > 1)
        {
            rejectedWater = waterLevel - 1;
            waterLevel = 1;
        }
        return rejectedWater;
    }

    private void SendWaterToChildren()
    {
        bool exitLoop = false;
        while (!exitLoop)
        {
            exitLoop = true;
            if (connectionPoint1.IsOpen() && connectionPoint2.IsOpen())
            {
                float halfWaterDiff = Mathf.Min((waterLevel - 0.5f) / 2f, connectionPoint1.capacity);
                float waterRejected1 = connectionPoint1.SendWater(halfWaterDiff);
                waterLevel -= (halfWaterDiff - waterRejected1);
                float waterRejected2 = connectionPoint2.SendWater(halfWaterDiff);
                waterLevel -= (halfWaterDiff - waterRejected2);
            }
            else if (connectionPoint1.IsOpen())
            {
                float waterDiff = Mathf.Min(waterLevel - 0.5f, connectionPoint1.capacity);
                float waterRejected = connectionPoint1.SendWater(waterDiff);
                waterLevel -= (waterDiff - waterRejected);
                exitLoop = false;
            }
            else if (connectionPoint2.IsOpen())
            {
                float waterDiff = Mathf.Min(waterLevel - 0.5f, connectionPoint2.capacity);
                float waterRejected = connectionPoint2.SendWater(waterDiff);
                waterLevel -= (waterDiff - waterRejected);
                exitLoop = false;
            }

            if (waterLevel <= 0.5f)
                exitLoop = true;
        }
    }

    public Vector3 GetConnectPosition()
    {
        return transform.localPosition + new Vector3(0, 0.47f, 0);
    }
}
