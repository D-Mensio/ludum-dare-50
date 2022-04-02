using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceBucket : MonoBehaviour
{
    public Bucket bucket;

    public float amountPerSecond;
    public float startingAmountPerSecond;
    public float maxAmountPerSecond;

    public float accellerationTime;
    private float timePassedFromStart;

    void Update()
    {
        timePassedFromStart += Time.deltaTime;
        if (timePassedFromStart < accellerationTime)
        {
            float coeff = timePassedFromStart / accellerationTime;
            amountPerSecond = coeff * maxAmountPerSecond + (1 - coeff) * startingAmountPerSecond;
        }
        else
            amountPerSecond = maxAmountPerSecond;
            


        float rejected = bucket.ReceiveWater(amountPerSecond * Time.deltaTime);
        if (rejected > 0)
            Debug.Log("Lost");
    }
}
