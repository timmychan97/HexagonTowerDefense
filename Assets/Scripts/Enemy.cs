using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform goal;
    public static int numEnemies;
    private int id;
    public int atkRange;
    public int atk;
    public int maxHp;
    private int hp;
    public int worth;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        hp = maxHp;
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
        // Debug.Log("Original HP: " + hp.ToString());
        hp -= dmg;
        // Debug.Log("Lost HP, remaining: " + hp.ToString());
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
}
