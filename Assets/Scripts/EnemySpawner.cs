using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPre;
    public float enemySpawnInterval = 6f;
    public float spawnCountdown = 1f;
    public Transform spawnLocation;
    public Transform goal;
    public EnemySpawner es;
    public int cnt;
    private int numEnemies;
    void Start()
    {
        cnt = 1;
    }
    void Update()
    {
        if (spawnCountdown <= 0f)
        {
            NewRound();
            spawnCountdown = enemySpawnInterval;
            GameController.INSTANCE.OnRoundStart();
        }
        spawnCountdown -= Time.deltaTime;
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
        cnt++;
    }
}