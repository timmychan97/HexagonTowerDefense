using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager INSTANCE;
    HashSet<Unit> units = new HashSet<Unit>();

    void Awake()
    {
        INSTANCE = this;
    }

    public void OnBuyUnit(Unit unit)
    {
        units.Add(unit);
        GameController.INSTANCE.OnBuyGameUnit(unit);
    }

    public int GetUnitCount() => units.Count;

    public void ClearAllUnits()
    {
        foreach (Unit u in units) 
            if (u != null) units.Remove(u);
        units.Clear();
    }
}
