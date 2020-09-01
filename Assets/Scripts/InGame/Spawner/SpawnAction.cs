using UnityEngine;

public class SpawnAction
{
    public Transform unit = null;
    public int cnt = 0;
    public float delay = 0.0f; // TODO
    
    public void SetSpawnUnit(Transform unit, int cnt) {
        this.unit = unit;
        this.cnt = cnt;
    }
}
