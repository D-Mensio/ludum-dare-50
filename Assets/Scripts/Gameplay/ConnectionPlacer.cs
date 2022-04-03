using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionPlacer : MonoBehaviour
{
    public int connectionsAvailable;

    public List<ConnectionPoint> connectionsPoints;

    public GameObject connectionPrefab;

    public bool placingConnection;

    public ConnectionPoint cp;

    public bool selectedCp;

    public LayerMask bucketLayerMask, connectionPointLayerMask;

    public float refillTime;
    float lastRefill;

    public TMPro.TextMeshProUGUI connectionNumberIndicator;
    public Image fillImage;

    void Update()
    {
        //refill
        connectionNumberIndicator.text = connectionsAvailable.ToString();
        float progress = (Time.time - lastRefill) / refillTime;
        fillImage.fillAmount = progress;
        if (progress >= 1)
        {
            lastRefill = Time.time;
            connectionsAvailable++;
        }

        //place connection
        if (placingConnection && Input.GetMouseButtonDown(0))
        {
            if (selectedCp)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, bucketLayerMask);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Bucket"))
                    {
                        Bucket hitBucket = hit.collider.gameObject.GetComponent<Bucket>();
                        if (cp.ValidateConnection(hitBucket.GetConnectPosition()))
                            PlaceConnection(cp, hitBucket);
                        else
                            Debug.Log("Invalid Bucket");
                    }
                }
            }
            else
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, connectionPointLayerMask);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("ConnectionPoint"))
                    {
                        cp = hit.collider.gameObject.GetComponent<ConnectionPoint>();
                        selectedCp = true;
                        ShowConnections(false);
                        break;
                    }
                }
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
        foreach (var cp in connectionsPoints)
        {
            cp.Show(show);
        }
    }

}
