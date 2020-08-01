using UnityEngine;

public class UnitRangeMarker : MonoBehaviour
{
    // There is only one and the same unit range marker.
    // One marker is reused over and over.

    public static UnitRangeMarker INSTANCE;

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


    public static void ShowTowerRangeMarkerOnTower(Unit tower)
    {
        if (UnitRangeMarker.INSTANCE)
        {
            if (tower != null)
            {
                _ShowTowerRangeMarker(tower);
            }
            else
            {
                INSTANCE.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("There are no TowerRangeMarker on scene. Make sure to add one.");
        }
    }

    private static void _ShowTowerRangeMarker(Unit tower)
    {
        var trm = INSTANCE;
        trm.gameObject.SetActive(false);
        trm.SetRadius(tower.GetRange());
        trm.transform.position = tower.transform.position;
        trm.gameObject.SetActive(true);
    }
}
