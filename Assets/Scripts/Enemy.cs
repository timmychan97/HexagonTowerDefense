using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IDestroyable
{
    public HealthBarPivot healthBarPivot;
    public Transform goal;
    public static int numEnemies;
    private int id;
    public int atkRange;
    public int atk;
    public int maxHp;
    private int hp;
    public int worth;
    private static int maxId = 0;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
            agent.destination = goal.position;
        hp = maxHp;
        id = maxId;
        maxId++;

        // Initialize the healthBar
        healthBarPivot.AddUIHealthBar();
        healthBarPivot.SetMaxHealth(maxHp);
        healthBarPivot.SetHealth(maxHp);
        id = numEnemies;
        numEnemies++;
    }

    // Update is called once per frame
    void Update()
    {
    }


    // returns false when enemy does not lose hp
    public bool LoseHealth(int dmg)
    {
        hp -= dmg;
        healthBarPivot.SetHealth(hp);

        if (hp <= 0) 
        {
            Die();
        }
        return true; 
    }

    public void Die()
    {
        Debug.Log("Enemy " + id.ToString() + " dies");
        GameController.INSTANCE.GainReward(worth);

        Destroy(gameObject);
    }

    public int GetId() { return id; }




    public void Destroy()
    {
        Die();
    }

    public void TakeDamage(float health)
    {
        LoseHealth(Mathf.RoundToInt(health));
    }
}
