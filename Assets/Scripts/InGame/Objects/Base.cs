
using UnityEngine;

public class Base : GameUnit
{
    public new void Die(AttackInfo attackInfo)
    {
        Debug.LogWarning("Base die animation not implemented");
        // display die animation
    }

    public new void TakeDmg(AttackInfo attackInfo)
    {
        GameController.INSTANCE?.OnBaseTakeDmg(this);
        base.TakeDmg(attackInfo);
    }
}
