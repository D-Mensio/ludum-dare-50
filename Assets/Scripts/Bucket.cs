using UnityEngine;

public class Bucket : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private ConnectionPoint connectionPoint1;
    [SerializeField]
    private ConnectionPoint connectionPoint2;
    public float waterLevel = 0;
    public bool isSource = false;
    private float waterReceived;
    private float lerpedWaterReceived;

    [Header("Object Connections")]
    [SerializeField]
    private GameObject water;   
    [SerializeField]
    private ParticleSystem waterParticles;

    void LateUpdate()
    {
        water.transform.localScale = new Vector3(1, waterLevel, 1);
        var main = waterParticles.main;
        if (Time.deltaTime != 0)
        {
            lerpedWaterReceived = Mathf.Lerp(lerpedWaterReceived, waterReceived * 3 / Time.deltaTime, 4 * Time.deltaTime);
            lerpedWaterReceived = Mathf.Clamp(lerpedWaterReceived, 0, 0.2f);
            main.startSizeMultiplier = lerpedWaterReceived;
        }
    }

    private void Update()
    {
        waterReceived = 0;
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
        waterReceived += amount - rejectedWater;
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
                float halfWaterDiff = Mathf.Min((waterLevel - 0.5f) / 2f, connectionPoint1.currentCapacity);
                float waterRejected1 = connectionPoint1.SendWater(halfWaterDiff);
                waterLevel -= (halfWaterDiff - waterRejected1);
                float waterRejected2 = connectionPoint2.SendWater(halfWaterDiff);
                waterLevel -= (halfWaterDiff - waterRejected2);
            }
            else if (connectionPoint1.IsOpen())
            {
                float waterDiff = Mathf.Min(waterLevel - 0.5f, connectionPoint1.currentCapacity);
                float waterRejected = connectionPoint1.SendWater(waterDiff);
                waterLevel -= (waterDiff - waterRejected);
                exitLoop = false;
            }
            else if (connectionPoint2.IsOpen())
            {
                float waterDiff = Mathf.Min(waterLevel - 0.5f, connectionPoint2.currentCapacity);
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
        return transform.position + new Vector3(0, 0.47f, 0);
    }

    public bool TryGetClosestConnectionPoint(out ConnectionPoint chosenCp, Vector3 pos)
    {
        chosenCp = null;
        if (connectionPoint2.isConnected && connectionPoint1.isConnected)
            return false;
        else if (connectionPoint2.isConnected)
        {
            chosenCp = connectionPoint1;
        }
        else if (connectionPoint1.isConnected)
            chosenCp = connectionPoint2;
        else if (pos.x - transform.position.x >= 0)
        {
            chosenCp = connectionPoint2;
        }
        else
        {
            chosenCp = connectionPoint1;
        }
        return true;
    }
}
