using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IDestroyable
{
    public HealthBarPivot healthBarPivot;
    public Transform goal;
    public static int maxId;
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
        if (agent)
            agent.destination = goal.position;
        hp = maxHp;
        id = maxId;
        maxId++;

        healthBarPivot.MaxHealth = maxHp;
        healthBarPivot.Health = maxHp;
        Debug.Log("Maxhp: " + healthBarPivot.MaxHealth);
        Debug.Log("HP: " + healthBarPivot.Health);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            LoseHealth(10);
        }
    }

    // returns false when enemy does not lose hp
    public bool LoseHealth(int dmg)
    {
        Debug.Log("Original HP: " + hp.ToString());
        hp -= dmg;


        healthBarPivot.Health -= dmg;
        Debug.Log("Lost HP, remaining: " + hp.ToString());
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
