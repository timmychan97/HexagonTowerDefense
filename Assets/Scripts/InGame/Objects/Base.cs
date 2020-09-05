
using UnityEngine;

public class Base : GameUnit
{
    public override void Die(AttackInfo attackInfo)
    {
        Debug.LogWarning("Base die animation not implemented");
        // display die animation
    }

    public override void TakeDmg(AttackInfo attackInfo)
    {
        GameController.INSTANCE?.OnBaseTakeDmg(this);
        base.TakeDmg(attackInfo);
    }
}
