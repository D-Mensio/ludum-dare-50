using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionPlacer : MonoBehaviour
{
    public int connectionsAvailable;

    public List<ConnectionPoint> connectionsPoints;

    public GameObject connectionPrefab;

    public bool placingConnection;

    ConnectionPoint cp;

    public bool selectedCp;

    public LayerMask bucketLayerMask, connectionPointLayerMask;

    void Update()
    {
        if (placingConnection && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit;
            if (selectedCp)
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, bucketLayerMask);
                if (hit.collider != null)
                {
                    Bucket hitBucket = hit.transform.gameObject.GetComponent<Bucket>();
                    if (cp.ValidateConnection(hitBucket.GetConnectPosition()))
                        PlaceConnection(cp, hitBucket);
                    else
                        Debug.Log("Invalid Bucket");
                }
                else
                    Debug.Log("Not a bucket");
            }
            else
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, connectionPointLayerMask);
                if (hit.collider != null)
                {
                    cp = hit.transform.gameObject.GetComponent<ConnectionPoint>();
                    selectedCp = true;
                    ShowConnections(false);
                }
                else
                    Debug.Log("Not a connection point");
            }
        }
    }

    private void PlaceConnection(ConnectionPoint p, Bucket b)
    {
        LineRenderer lr = Instantiate(connectionPrefab, new Vector3(0, 0, 2), Quaternion.identity).GetComponent<LineRenderer>();
        lr.SetPosition(0, p.transform.position);
        lr.SetPosition(1, b.GetConnectPosition());
        p.CreateConnection(b);
        connectionsPoints.Remove(p);
        connectionsAvailable--;
        placingConnection = false;
        selectedCp = false;
    }

    public void StartPlacingConnection()
    {
        if (connectionsAvailable > 0)
        {
            placingConnection = true;
            ShowConnections(true);
        }
        else
            Debug.Log("Not enough connections!");
    }

    public void StopPlacingConnection()
    {
        if (placingConnection)
        {
            ShowConnections(false);
            placingConnection = false;
            selectedCp = false;
            cp = null;
        }
    }

    public void ShowConnections(bool show)
    {
        Vector3 newScale = Vector3.zero;
        if (show)
            newScale = new Vector3(0.3f, 0.3f, 1);
        foreach (var cp in connectionsPoints)
        { 
            cp.transform.localScale = newScale;
        }
    }

}
