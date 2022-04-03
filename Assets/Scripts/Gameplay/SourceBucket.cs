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

    private bool alertActive;

    public ParticleSystem alert;

    private void Awake()
    {
        alert.Stop();
        timePassedFromStart = 0f;
    }

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
        {
            GameOverManager.Instance.GameOver(transform.position);
        }
        if (!alertActive && bucket.waterLevel > 0.7f)
        {
            alertActive = true;
            alert.Play();
        }
        else if (alertActive && bucket.waterLevel < 0.6f)
        {
            alertActive = false;
            alert.Stop();
        }
    }
}
