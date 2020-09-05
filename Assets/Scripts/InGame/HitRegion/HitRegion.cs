using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRegion : MonoBehaviour
{

    protected HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    protected Effect effect;

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy) enemiesInRange.Add(enemy);
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy) enemiesInRange.Remove(enemy);
    }

    public void ApplyEffect(Effect effect) 
    {
        foreach (Enemy enemy in enemiesInRange)
            if (enemy) enemy.TakeEffect(effect);
    }

    public HashSet<Enemy> GetEnemiesInRange()
    {
        return enemiesInRange;
    }

    public void SetEffect(Effect _effect) => effect = _effect;
    public void SetPos(Vector3 pos) => transform.position = pos;
}
