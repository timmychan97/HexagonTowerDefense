using UnityEngine;

public class TowerRangeMarker : MonoBehaviour
{
    // There is only one and the same tower range marker.
    // One marker is reused over and over.

    public static TowerRangeMarker INSTANCE;

    public float rotationsPerMinute = 7f;

    void Start()
    {
        INSTANCE = this;
        gameObject.SetActive(false);
    }

    void Update()
    {
        // 6f is 360/60
        transform.Rotate(0, 0, 6f * rotationsPerMinute * Time.deltaTime);
    }

    public void SetRadius(float radius)
    {
        // Scale of the marker is 1 in diameter
        var scale = radius * 2f;
        transform.localScale = new Vector3(scale, scale, scale);
    }


    public static void MoveToTower(Tower tower)
    {
        if (INSTANCE)
        {
            if (tower != null)
            {
                _ShowTowerRangeMarker(tower);
            }
            else
            {
                Hide();
            }
        }
        else 
        {
            LogNotFound();
        }
    }

    public static void Hide() {
        if(INSTANCE) {
            INSTANCE.gameObject.SetActive(false);
        } else {
            LogNotFound();
        }
    }

    public static void Show() {
        if(INSTANCE) {
            INSTANCE.gameObject.SetActive(true);
        } else {
            LogNotFound();
        }
    }

    private static void _ShowTowerRangeMarker(Tower tower)
    {
        var trm = INSTANCE;
        trm.SetRadius(tower.GetRange());
        trm.transform.position = tower.transform.position;
    }

    private static void LogNotFound() {
        Debug.LogWarning("There are no TowerRangeMarker on scene. Make sure to add one.");
    }
}
