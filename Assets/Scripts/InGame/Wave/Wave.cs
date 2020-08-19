using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    private List<SpawnAction> spawnActions = new List<SpawnAction>();
    private int idx = 0;

    public void AddUnit(Transform unit, int cnt)
    {
        SpawnAction action = new SpawnAction();
        action.SetSpawnUnit(unit, cnt);
        spawnActions.Add(action);
    }

    public void AddSpawnAction(SpawnAction spawnAction) => spawnActions.Add(spawnAction);

    public List<SpawnAction> GetSpawnActions() => spawnActions;

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
