using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public float enemySpawnInterval;
    public float spawnCountdown;
    public Transform spawnLocation;
    public Transform goal;
    public EnemySpawner es;
    private int numEnemies;
    private int numRounds;
    HashSet<Round> ongoingRounds;
    HashSet<Enemy> enemies;
    void Start()
    {
        ongoingRounds = new HashSet<Round>();
        enemies = new HashSet<Enemy>();
    }
    void Update()
    {
        HandleSpawning();
    }

    void HandleSpawning()
    {
        List<Round> toRemove = new List<Round>(); // remove rounds that have no more spawn actions waiting
        foreach (Round round in ongoingRounds) 
        {
            SpawnAction nextAction = round.NextSpawnAction();
            if (nextAction == null) 
            {
                toRemove.Add(round);
                break;
            } 
            else 
            {
                DoSpawnAction(nextAction);
            }
        }
        foreach (Round round in toRemove) 
        {
            ongoingRounds.Remove(round);
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
        e.target = GameController.INSTANCE.myBase;
        enemies.Add(e);
    }

    public void StartRound(Round round) 
    {
        ongoingRounds.Add(round);
    }

    public HashSet<Enemy> GetEnemies() { 
        UpdateEnemySet();
        return enemies; 
    }
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
}