using UnityEngine;

public class UnitRangeMarker : MonoBehaviour
{
    // There is only one and the same tower range marker.
    // One marker is reused over and over.

    public static UnitRangeMarker INSTANCE;

    public float rotationsPerMinute = 7f;

    void Awake() => INSTANCE = this;

    void Start() => gameObject.SetActive(false);

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
        if(unit)
            _ShowUnitRangeMarker(unit);
        else
            Hide();
    }

    public static void Hide() => INSTANCE.gameObject.SetActive(false);

    public static void Show() => INSTANCE.gameObject.SetActive(true);

    private static void _ShowUnitRangeMarker(GameUnit gameUnit)
    {
        var urm = INSTANCE;
        Unit unit = gameUnit as Unit;
        if (unit != null)
        {
            urm.SetRadius(unit.GetAttackRadius());
            urm.transform.position = unit.transform.position;
        }
    }
}
