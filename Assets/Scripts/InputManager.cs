using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public ConnectionPlacer cp;
    public BucketPlacer bp;

    private bool paused = false;
    private bool doubleSpeed = false;

    public bool gameOver;

    public void Update()
    {
        if (!gameOver)
        {
            //Pause/Unpause
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (paused)
                    Play();
                else
                    Pause();
            }


            //Cancel bucket placement
            if (bp.placingBucket && Input.GetMouseButtonDown(1))
            {
                bp.StopPlacingBucket();
            }
            //Cancel connection placement
            else if (cp.placingConnection && Input.GetMouseButtonDown(1))
            {
                cp.StopPlacingConnection();
            }
            //Start bucket placement
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (cp.placingConnection)
                    cp.StopPlacingConnection();
                if (!bp.placingBucket)
                    bp.StartPlacingBucket();
            }
            //Start connection placement
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (bp.placingBucket)
                    bp.StopPlacingBucket();
                if (!cp.placingConnection)
                    cp.StartPlacingConnection();
            }
        }
    }

    public void Pause()
    {
        if (!gameOver)
        {
            Time.timeScale = 0;
            paused = true;
        }
    }

    public void SetNormalSpeed()
    {
        if (!gameOver)
        {
            doubleSpeed = false;
            Play();
        }
    }

    public void SetDoubleSpeed()
    {
        if (!gameOver)
        {
            doubleSpeed = true;
            Play();
        }
    }

    public void Play()
    {
        if (!gameOver)
        {
            paused = false;
            if (doubleSpeed)
                Time.timeScale = 2f;
            else
                Time.timeScale = 1f;
        }
    }

    public void GameOver()
    {
        gameOver = true;
        bp.StopPlacingBucket();
        cp.StopPlacingConnection();
    }

}
