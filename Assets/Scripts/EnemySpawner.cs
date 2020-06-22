using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPre;

    public float enemySpawnInterval = 10f;
    public float spawnCountdown = 2f;
    public Transform spawnLocation;
    public Transform goal;
    public EnemySpawner es;

    private int numberOfEnemies;
    void Update()
    {
        if (spawnCountdown <= 0f)
        {
            SpawnEnemy();
            spawnCountdown = enemySpawnInterval;
        }
        spawnCountdown -= Time.deltaTime;   // reduced by 1 every second
    }

    void SpawnEnemy()
    {
        var a = Instantiate(enemyPre, spawnLocation);
        Enemy e = a.GetComponent<Enemy>();
        e.goal = goal;
    }
}