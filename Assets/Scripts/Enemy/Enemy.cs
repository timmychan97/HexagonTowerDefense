using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : GameUnit, IDamagable, IAttackable, IDestroyable, IAffectable, IPropertiesDisplayable
{
    public HealthBarPivot healthBarPivot;
    public GameUnit goal;
    public GameUnit target = null;

    /* stats */
    protected int id;
    public int level;
    public float atkRange;
    public int atk;
    public float moveSpeed;
    public float atkSpeed; // in Hz
    public int worth;
    float atkRangeSqr;
    float atkPeriod;
    float atkCountdown;
    float lastAtkTime;

    public static int totalNumEnemies = 0;
    private NavMeshAgent navMeshAgent;
    public HashSet<Effect> effects;
    EnemyRange range;
    // Start is called before the first frame update
    protected void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        if (navMeshAgent)
            navMeshAgent.destination = goal.transform.position;
        hp = maxHp;
        id = totalNumEnemies;
        totalNumEnemies++;
        atkRangeSqr = atkRange * atkRange;
        atkPeriod = 1f / atkSpeed;
        lastAtkTime = Time.time;
        atkCountdown = atkPeriod;

        effects = new HashSet<Effect>();

        // Initialize the healthBar
        healthBarPivot.AddUIHealthBar();
        healthBarPivot.SetMaxHealth(maxHp);
        healthBarPivot.SetHealth(maxHp);

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.INSTANCE.IsGamePlaying()) return;
        HandleAtk();
    }

    public void Init()
    {
        // Initialize member variables
        // We might need them before instantiation (i.e. before Start() is called)
        hp = maxHp;
        range = Instantiate(GameController.INSTANCE.pf_enemyRange, transform);
        // unitRange = t.GetComponent<UnitRange>();
        if (range == null)
        {
            Debug.LogWarning("Unit Range prefab has no UnitRange script attached to it");
        }
        range.Init(this);
    }

    public void HandleAtk()
    {
        if (IsGoalInRange())
        {
            Debug.Log("Goal is in range");
            // attack goal (base)
            atkCountdown -= Time.deltaTime;
            if (atkCountdown < 0){
                Atk(goal);
                atkCountdown = atkPeriod;
            }
        }
        else if (target != null)
        {
            // check if this enemy can attack units
            atkCountdown -= Time.deltaTime;
            if (atkCountdown < 0) 
            {
                Atk(target);
                atkCountdown = atkPeriod;
            }
        }
    }

    bool IsGoalInRange()
    {
        if (goal == null) return false;
        float dist2 = (goal.transform.position - transform.position).sqrMagnitude;
        return atkRangeSqr > dist2;
    }

    public void Destroy()
    {

    }

    public override void Die()
    {
        GameController.INSTANCE.OnEnemyDie(this);
        Destroy(gameObject);
    }

    public int GetId() => id;

    public virtual void Atk(GameUnit gu)
    {
        // By default, just deal damage
        gu.TakeDmg(atk);
    }

    public override void TakeDmg(float dmg)
    {
        base.TakeDmg(dmg);
        healthBarPivot.SetHealth(hp);
    }

    public override void TakeEffect(Effect effect)
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

    public new UI_PanelUnitInfo GetPanelUnitInfo() 
    {
        // get the prefab for Enemy class from UI_PanelUnitInfoManager, 
        // link it with this object, then return the panel
        UI_PanelUnitInfo_Enemy panel = UI_PanelUnitInfoManager.INSTANCE.pf_enemyPanel;
        panel.SetEnemy(this);
        return panel; 
    }

    public GameUnit GetTarget() 
    { 
        return target; 
    }

    public void SetTarget(GameUnit gameUnit) 
    {
        target = gameUnit; 
    }


    public int GetAtk() 
    {
        return atk;
    }

    public void SetAtk(int a)
    {
        atk = a;
    }

    public float GetAtkRange()
    {
        return atkRange;
    }

    public void SetAtkRange(float a)
    {
        atkRange = a;
    }
}
