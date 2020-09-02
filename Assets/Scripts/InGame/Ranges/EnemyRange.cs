using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Range
{
    Enemy enemy;

    public override void Init(GameUnit gu, float radius)
    {
        base.Init(gu, radius);
        Init((Enemy)gu);
    }

    public void Init(Enemy e) => enemy = e;

    /// <returns>
    /// Whether the given GameUnit is a potential target for
    /// the GameUnit that this Range belongs to.
    /// </returns>
    public override bool IsPotentialTarget(GameUnit gu)
    {
        if (base.IsPotentialTarget(gu))
        {
            if (gu is Unit || gu is Building)
            {
                return true;
            }
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
        enemy.SetAttackTarget(gu);
    }
}
