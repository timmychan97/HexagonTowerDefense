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
        enemy.SetTarget(gu);
    }
}
