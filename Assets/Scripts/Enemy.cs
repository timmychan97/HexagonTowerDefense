using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IDestroyable
{
    public HealthBarPivot healthBarPivot;
    public Transform goal;
    public IDamagable target;
    private int id;
    public float atkRange;
    private float atkRangeSqr;
    public int atk;
    public float atkSpeed; // in Hz
    private float atkPeriod;
    private float lastAtkTime;
    public int maxHp;
    private int hp;
    public int worth;
    public static int totalNumEnemies = 0;
    public Vector3 prevPos;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent)
            navMeshAgent.destination = goal.position;
        hp = maxHp;
        id = totalNumEnemies;
        totalNumEnemies++;
        atkRangeSqr = atkRange * atkRange;
        atkPeriod = 1f / atkSpeed;
        lastAtkTime = Time.time;

        // Initialize the healthBar
        healthBarPivot.AddUIHealthBar();
        healthBarPivot.SetMaxHealth(maxHp);
        healthBarPivot.SetHealth(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAtk();
    }

    void LateUpdate()
    {
        prevPos = transform.position;
    }

    public void HandleAtk()
    {
        if (Time.time < lastAtkTime + atkPeriod) return;

        lastAtkTime = Time.time;
        float dist2 = (goal.position - transform.position).sqrMagnitude;
        if (atkRangeSqr > dist2) // can attack goal
        {
            if (target != null) 
            {
                if (target is Base)
                {
                    Atk(target);
                }
            }
        }
    }

    public void Die()
    {
        Debug.Log("Enemy " + id.ToString() + " dies");
        GameController.INSTANCE.GainReward(worth);
        Destroy(gameObject);
    }

    public int GetId() { return id; }

    public void Atk(IDamagable b)
    {
        Debug.Log("attack");
        b.TakeDmg(atk);
    }

    public void Destroy()
    {
        
    }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
        healthBarPivot.SetHealth(hp);
        if (hp <= 0)
        {
            Die();
        }
    }

    public Vector3 GetVelocity()
    {
        if (navMeshAgent == null) return Vector3.zero;
        return navMeshAgent.velocity;
    }
}
