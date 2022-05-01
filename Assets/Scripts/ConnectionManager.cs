using System;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]   
    private int connectionsAvailable;
    [SerializeField]
    private float refillTime;
    private float lastRefill;
    [SerializeField]
    private GameEvent hideBuckets;
    [SerializeField]
    private ConnectionPointEvent showBuckets;
    [SerializeField]
    private GameObject connectionPrefab, connectionEndPointPrefab;
    [SerializeField]
    private LayerMask bucketLayerMask, connectionPointLayerMask;

    [Header("Object Connections")]
    [SerializeField]
    private TMPro.TextMeshProUGUI connectionNumberIndicator;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private Animator connectionResourceAC;

    //Others
    [NonSerialized]
    public bool placingConnection, selectedCp;
    private LineRenderer lr;
    private ConnectionPoint cp;


    void Update()
    {
        Refill();

        if (placingConnection)
        {
            if (Input.GetMouseButtonUp(0))
            {
                SimplifiedSelectBucket();
            }

        }
    }

    private void LateUpdate()
    {
        if (placingConnection && selectedCp)
        {
            UpdateLineRenderer();
        }
    }

    private void Refill()
    {
        connectionNumberIndicator.text = connectionsAvailable.ToString();
        float progress = (Time.time - lastRefill) / refillTime;
        fillImage.fillAmount = progress;
        if (progress >= 1)
        {
            lastRefill = Time.time;
            connectionsAvailable++;
        }
    }

    private void UpdateLineRenderer()
    {
        bool colorSet = false;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        pos = new Vector3(pos.x, pos.y, 2);

        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, bucketLayerMask);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Bucket"))
            {
                Bucket hitBucket = hit.collider.gameObject.GetComponent<Bucket>();
                Vector3 connectPos = hitBucket.GetConnectPosition();
                if (!hitBucket.isSource && cp.ValidateConnection(connectPos))
                {
                    //SetLrColor(Color.green);
                    //colorSet = true;
                    pos = hitBucket.GetConnectPosition();
                }
                break;
            }
        }

        lr.SetPosition(1, pos);
        if (!colorSet)
        {
            if (cp.ValidateConnection(pos))
                SetLrColor(Color.black);
            else
                SetLrColor(Color.red);
        }
    }

    private void SetLrColor(Color c)
    {
        lr.startColor = c;
        lr.endColor = c;
    }

    private void SimplifiedSelectBucket()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapBox(mousePos, new Vector2(1, 1), 0, bucketLayerMask);
        if (hitCollider)
        {
            Bucket hitBucket = hitCollider.gameObject.GetComponent<Bucket>();
            if (!hitBucket.isSource && cp.ValidateConnection(hitBucket.GetConnectPosition()))
            {
                FinishConnection(cp, hitBucket);
            }
            else
                StopPlacingConnection();
        }
        else
            StopPlacingConnection();
    }

    private void FinishConnection(ConnectionPoint p, Bucket b)
    {
        Vector3 pos = b.GetConnectPosition();
        pos = new Vector3(pos.x, pos.y, 2);
        lr.SetPosition(1, pos);
        GameObject endpoint = Instantiate(connectionEndPointPrefab, lr.transform);
        endpoint.transform.position = pos;
        p.CreateConnection(b);
        connectionsAvailable--;
        SetLrColor(Color.black);
        ShowBuckets(false);
        lr = null;
        placingConnection = false;
        selectedCp = false;
    }

    public void StartPlacingConnection(ConnectionPoint cp)
    {
        if (connectionsAvailable > 0)
        {
            placingConnection = true;
            selectedCp = true;
            this.cp = cp;
            lr = Instantiate(connectionPrefab, new Vector3(0, 0, 2), Quaternion.identity).GetComponent<LineRenderer>();
            Vector3 pos = cp.transform.position;
            pos = new Vector3(pos.x, pos.y, 2);
            lr.SetPosition(0, pos);
            GameObject endpoint = Instantiate(connectionEndPointPrefab, lr.transform);
            endpoint.transform.position = pos;
            ShowBuckets(true);
        }
        else
        {
            //Not enough connections
            connectionResourceAC.SetTrigger("FlashRed");
        }

    }



    public void StopPlacingConnection()
    {
        if (placingConnection)
        {
            ShowBuckets(false);
            placingConnection = false;
            selectedCp = false;
            cp = null;
            if (lr != null)
            {
                Destroy(lr.gameObject);
                lr = null;
            }
        }
    }


    public void ShowBuckets(bool show)
    {
        if (show)
            showBuckets.Raise(cp);
        else
            hideBuckets.Raise();
    }

}
