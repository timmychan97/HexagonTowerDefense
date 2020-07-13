using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round
{
    private List<SpawnAction> spawnActions = new List<SpawnAction>();
    private int idx = 0;

    public void AddUnit(Transform unit, int cnt)
    {
        SpawnAction action = new SpawnAction();
        action.SetSpawnUnit(unit, cnt);
        spawnActions.Add(action);
    }

    public List<SpawnAction> GetSpawnActions()
    {
        return spawnActions;
    }

    public SpawnAction NextSpawnAction() 
    {
        if (idx >= spawnActions.Count) return null;
        
        SpawnAction action = spawnActions[idx];
        idx++;
        return action;
    }

    public bool IsOver()
    {
        return idx > spawnActions.Count;
    }
}
