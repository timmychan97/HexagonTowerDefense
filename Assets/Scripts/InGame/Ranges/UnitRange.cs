using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : Range
{
    Unit unit;

    public override void Init(GameUnit gu, float radius)
    {
        base.Init(gu, radius);
        Init((Unit)gu);
    }

    public void Init(Unit u) => unit = u;

    /// <returns>
    /// Whether the given GameUnit is a potential target for
    /// the GameUnit that this Range belongs to.
    /// </returns>
    public override bool IsPotentialTarget(GameUnit gu)
    {
        return base.IsPotentialTarget(gu) && (gu is Enemy);
    }

    /// <summary>
    /// Gets the current target of the GameUnit that this Range belongs to
    /// </summary>
    public override GameUnit GetCurrentTarget() => target;

    public override void SetTarget(GameUnit gu)
    {
        base.SetTarget(gu);
        unit.SetAttackTarget(gu);
    }
}
