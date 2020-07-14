using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable, IDestroyable, IAffectable
{
    public HealthBarPivot healthBarPivot;
    public Transform goal;
    public IDamagable target;
    private int id;
    public float atkRange;
    private float atkRangeSqr;
    public int atk;
    public float moveSpeed;
    public float atkSpeed; // in Hz
    private float atkPeriod;
    private float lastAtkTime;
    public int maxHp;
    private int hp;
    public int worth;
    public static int totalNumEnemies = 0;
    public Vector3 prevPos;
    private NavMeshAgent navMeshAgent;
    public HashSet<Effect> effects;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        if (navMeshAgent)
            navMeshAgent.destination = goal.position;
        hp = maxHp;
        id = totalNumEnemies;
        totalNumEnemies++;
        atkRangeSqr = atkRange * atkRange;
        atkPeriod = 1f / atkSpeed;
        lastAtkTime = Time.time;

        effects = new HashSet<Effect>();

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

    public void Destroy()
    {

    }

    public void Die()
    {
        Debug.Log("Enemy " + id.ToString() + " dies");
        GameController.INSTANCE.GainReward(worth);
        Destroy(gameObject);
    }

    public int GetId() => id;

    public void Atk(IDamagable b)
    {
        Debug.Log("Enemy " + id.ToString() +  " attacks base");
        b.TakeDmg(atk);
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

    public void TakeEffect(Effect effect)
    {
        if (effect.stacked) 
        {
            AddEffect(effect);
        }
        else 
        {
            Effect e = GetEffectWithId(effect.id);
            if (e != null) // already has this effect
            {
                e.ResetCountdown();
            } 
            else  // does not have this effect, add it as a new effect
            {
                AddEffect(effect);
            }
        }
    }

    public void AddEffect(Effect effect)
    {
        Effect e = Instantiate(effect, transform);
        effects.Add(e);  // will this work for AOE effects?
        e.SetAffected(this);
        moveSpeed *= e.speedScale;
        navMeshAgent.speed = moveSpeed;
    }

    public bool HasEffect(Effect effect)
    {
        return effects.Contains(effect); 
    }
    public bool HasEffectWithName(string name) 
    {
        foreach (Effect e in effects) 
        {
            if (e.effectName == name) return true;
        }
        return false;
    }
    public Effect GetEffectWithId(int id)
    {
        foreach (Effect e in effects) 
        {
            if (e.id == id) return e;
        }
        return null;
    }

    public void RemoveEffect(Effect effect)
    {
        effects.Remove(effect);
        // restore parameters (only speed for now)
        moveSpeed /= effect.speedScale;
        navMeshAgent.speed = moveSpeed;
    }

    public Vector3 GetVelocity()
    {
        if (navMeshAgent == null) return Vector3.zero;
        return navMeshAgent.velocity;
    }
}
