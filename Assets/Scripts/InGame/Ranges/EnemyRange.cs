using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Range
{
    Enemy enemy;

    public void Init(Enemy e)
    {
        Init(e, e.GetAtkRange());
        enemy = e;
    }

    /// <returns>
    /// Whether the given GameUnit is a potential target for
    /// the GameUnit that this Range belongs to.
    /// </returns>
    public override bool IsPotentialTarget(GameUnit gu)
    {
        if (!base.IsPotentialTarget(gu))
        {
            return false;
        }
        if (gu is Unit unit)
        {
            return true;
        }
        else if (gu is Building building)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the current target of the GameUnit that this Range belongs to
    /// </summary>
    public override GameUnit GetCurrentTarget() => target;

    public override void SetTarget(GameUnit gu)
    {
        base.SetTarget(gu);
        enemy.SetTarget(gu);
    }
}
