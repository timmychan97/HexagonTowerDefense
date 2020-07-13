using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPre;
    public Transform enemySprinter;
    public float enemySpawnInterval;
    public float spawnCountdown;
    public Transform spawnLocation;
    public Transform goal;
    public EnemySpawner es;
    public int cnt;
    private int numEnemies;
    private int numRounds;
    HashSet<Round> ongoingRounds;
    void Start()
    {
        ongoingRounds = new HashSet<Round>();
        cnt = 1;
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
    }

    void NewRound()
    {
        for (int i = 0; i < cnt; i++) 
        {
            Transform a = Instantiate(enemyPre, spawnLocation);
            Enemy e = a.GetComponent<Enemy>();
            e.goal = goal;
            e.target = GameController.INSTANCE.myBase;
        }
        ++cnt;
    }

    public void StartRound(Round round) 
    {
        ongoingRounds.Add(round);
    }
}