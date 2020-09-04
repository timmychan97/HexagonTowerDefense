using UnityEngine;

public class AttackRangeMarker : MonoBehaviour
{
    // There is only one and the same tower range marker.
    // One marker is reused over and over.

    public static AttackRangeMarker INSTANCE;

    public float rotationsPerMinute = 7f;

    private GameUnit followingUnit;

    void Awake() => INSTANCE = this;

    void Start() => gameObject.SetActive(false);

    void Update()
    {
        // 6f is 360/60
        transform.Rotate(0, 0, 6f * rotationsPerMinute * Time.deltaTime);
        if (followingUnit && !followingUnit.IsDead())
        {
            PrepareTransform(followingUnit);
            Show();
        }
    }

    public void SetRadius(float radius)
    {
        // Scale of the marker is 1 in diameter
        var scale = radius * 2f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public static void FollowUnit(GameUnit unit)
    {
        if (unit && !unit.IsDead())
        {
            PrepareTransform(unit);
            INSTANCE.followingUnit = unit;
            Show();
        }
        else
            Hide();
    }

    public static void Hide() => INSTANCE.gameObject.SetActive(false);

    public static void Show() => INSTANCE.gameObject.SetActive(true);

    private static void PrepareTransform(GameUnit gameUnit)
    {
        var urm = INSTANCE;
        AttackableGameUnit unit = gameUnit as AttackableGameUnit;
        if (unit != null)
        {
            urm.SetRadius(unit.GetAttackRadius());
            urm.transform.position = unit.transform.position;
        }
    }
}
