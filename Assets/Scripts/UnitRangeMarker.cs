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


    public static void ShowUnitRangeMarkerOnUnit(Unit unit)
    {
        if (UnitRangeMarker.INSTANCE)
        {
            if (unit != null)
            {
                _ShowUnitRangeMarker(unit);
            }
            else
            {
                INSTANCE.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("There are no UnitRangeMarker on scene. Make sure to add one.");
        }
    }

    private static void _ShowUnitRangeMarker(Unit unit)
    {
        var trm = INSTANCE;
        trm.gameObject.SetActive(false);
        trm.SetRadius(unit.GetRange());
        trm.transform.position = unit.transform.position;
        trm.gameObject.SetActive(true);
    }
}
