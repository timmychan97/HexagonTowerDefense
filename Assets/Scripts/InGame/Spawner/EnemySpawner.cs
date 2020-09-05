using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnLocation;
    public GameUnit goal;
    public EnemySpawner es;
    private int numEnemies;
    private int numWaves;
    HashSet<Wave> ongoingWaves = new HashSet<Wave>();
    HashSet<Enemy> enemies = new HashSet<Enemy>();

    void Update()
    {
        HandleSpawning();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.8f);
        Gizmos.DrawSphere(transform.position, 1f);
    }

    void HandleSpawning()
    {
        List<Wave> toRemove = new List<Wave>(); // remove waves that have no more spawn actions waiting
        foreach (Wave wave in ongoingWaves) 
        {
            SpawnAction nextAction = wave.NextSpawnAction();
            if (nextAction == null) 
            {
                toRemove.Add(wave);
                break;
            } 
            else 
            {
                DoSpawnAction(nextAction);
            }
        }
        foreach (Wave wave in toRemove) 
        {
            ongoingWaves.Remove(wave);
        }
    }

    void DoSpawnAction(SpawnAction action)
    {
        if (action.unit != null) 
        {
            for (int i = 0; i < action.cnt; i++)
            {
                SpawnUnit(action.unit);
            }
        }
    }

    void SpawnUnit(Transform unit)
    {
        Transform t = Instantiate(unit, spawnLocation);
        Enemy e = t.GetComponent<Enemy>();
        e.goal = goal;
        enemies.Add(e);
    }

    public void StartWave(Wave wave) 
    {
        ongoingWaves.Add(wave);
        HandleSpawning();
    }

    public HashSet<Enemy> GetEnemies() { 
        UpdateEnemySet();
        return enemies; 
    }

    public HashSet<Wave> GetOngoingWaves() => ongoingWaves;

    public void UpdateEnemySet() {
        // remove dead enemies from the Set
        List<Enemy> toRemove = new List<Enemy>();
        foreach (Enemy enemy in enemies) 
        {
            if (enemy == null) {
                toRemove.Add(enemy);
            }
        }
        foreach (Enemy enemy in toRemove)
        {
            enemies.Remove(enemy);
        }
    }

    public void ClearAll()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null) 
            {
                Destroy(enemy.gameObject);
            }
        }
        enemies.Clear();
        ongoingWaves.Clear();
    }
}