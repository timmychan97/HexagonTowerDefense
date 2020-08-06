using UnityEngine;

public class UnitRangeMarker : MonoBehaviour
{
    // There is only one and the same tower range marker.
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


    public static void MoveToUnit(GameUnit unit)
    {
        if (INSTANCE)
        {
            if (unit != null)
            {
                _ShowUnitRangeMarker(unit);
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

    private static void _ShowUnitRangeMarker(GameUnit gameUnit)
    {
        var urm = INSTANCE;
        var unit = (Unit)gameUnit;
        if (unit)
        {
            urm.SetRadius(unit.GetRange());
            urm.transform.position = unit.transform.position;
        }
    }

    private static void LogNotFound() {
        Debug.LogWarning("There are no UnitRangeMarker on scene. Make sure to add one.");
    }
}
