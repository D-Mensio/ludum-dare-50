using System;
using UnityEngine;

[ExecuteAlways]
public class UpdateLineRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private Vector3 secondPos;

    void Update()
    {
        lr.SetPosition(1, secondPos);
    }
}
