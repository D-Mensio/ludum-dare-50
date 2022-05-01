using UnityEngine;

public class BucketEntrance : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Bucket ownBucket;

    public void Show(ConnectionPoint cp)
    {
        if (!ownBucket.isSource && cp.ValidateConnection(transform.position))
        {
            Color c = sr.color;
            c.a = 1;
            sr.color = c;
        }
    }

    public void Hide()
    {
        Color c = sr.color;
        c.a = 0;
        sr.color = c;
    }
}
