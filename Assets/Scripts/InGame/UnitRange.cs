using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : Range
{
    Unit unit;
    // SphereCollider sphereCollider;
    // HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();

    public void Init(Unit u) 
    {
        /*
        Called by Unit as the end of its Init()
        */

        Init(u, u.GetAtkRange());
        unit = u;
    }

    public override bool IsPotentialTarget(GameUnit gu)
    {
        /*
        Return:
            Whether the given GameUnit is a potential target for
            the GameUnit that this Range belongs to.
        */
        if (!base.IsPotentialTarget(gu))
        {
            return false;
        }
        return (gu is Enemy);
    }

    public override GameUnit GetCurrentTarget()
    {
        /*
        Return:
            The current target of the GameUnit that this Range
            belongs to.
        */
        return target;
    }

    public override void SetTarget(GameUnit gu)
    {
        base.SetTarget(gu);
        unit.SetTarget(gu);
    }
}
