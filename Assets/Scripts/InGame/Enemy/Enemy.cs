using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Enemy : GameUnit, IDamagable, IAffectable, IPropertiesDisplayable
{
    public EnemyCharacter character;
    public Animator animator;
    public HealthBarPivot healthBarPivot;
    public GameUnit goal;
    public GameUnit target = null;

    /* stats */
    public int level;
    public float atkRange;
    public int attackDamage;
    public float moveSpeed;
    public float atkSpeed; // in Hz
    public int worth;

    float atkRangeSqr;
    float atkPeriod;
    float atkCountdown;

    private NavMeshAgent navMeshAgent;
    public HashSet<Effect> effects = new HashSet<Effect>();
    public EnemyRange pf_enemyRange;
    EnemyRange range;

    protected void Start()
    {
        ValidateAttachedObjects();
        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.speed = moveSpeed;
        if (navMeshAgent && goal)
            navMeshAgent.destination = goal.transform.position;

        hp = maxHp;
        atkRangeSqr = atkRange * atkRange;
        atkPeriod = 1f / atkSpeed;
        atkCountdown = 0;

        if (healthBarPivot) healthBarPivot.AddUIHealthBar(maxHp);

        Init();
    }

    public void Init()
    {
        // Initialize member variables
        // We might need them before instantiation (i.e. before Start() is called)
        hp = maxHp;
        range = Instantiate(pf_enemyRange, transform);
        // unitRange = t.GetComponent<UnitRange>();
        if (range == null)
        {
            Debug.LogWarning("Unit Range prefab has no UnitRange script attached to it");
        }

        range.Init(this);
    }


    void Update()
    {
        if (GameController.INSTANCE)
            if (GameController.INSTANCE.IsGamePlaying()) return;
        HandleAttack();
        if (Input.GetMouseButton(0))
        {
            int cameraDistance = 600;
            RaycastHit rh;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out rh, cameraDistance))
            {
                navMeshAgent.destination = rh.point;
            }
        }

        UpdateMovement();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDmg(40, new Vector3(3, -4, 4));
        //}

    }

    void UpdateMovement()
    {
        // When doing ragdoll, the system will disable the agent. Make sure we check if it is enabled before moving
        if (navMeshAgent.enabled)
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                character.Move(navMeshAgent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
    }


    /// <summary>
    /// This methods outputs warnings to the developers providing info 
    /// about what is not properly set up in inspector for this enemy
    /// </summary>
    private void ValidateAttachedObjects()
    {
        if (!healthBarPivot) Debug.LogWarning($"No HealthBarPivot is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector. No health will be shown for this enemy");
        if (!animator) Debug.LogWarning($"No Animator is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector. Animations will not work properly without it");

        if (!goal) Debug.LogWarning($"No goal transform is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector");
    }

    public void HandleAttack()
    {
        if (IsGoalInRange())
        {
            Debug.Log("Goal is in range");
            StopMoving();
            // attack goal (base)
            atkCountdown -= Time.deltaTime;
            if (atkCountdown < 0){
                Attack(goal);
                atkCountdown = atkPeriod;
            }
        }
        else if (target != null)
        {
            // Visualize target
#if UNITY_EDITOR
            Debug.DrawLine(transform.position + (Vector3.up * 1f), target.transform.position, Color.red);
#endif
            // check if this enemy can attack units
            atkCountdown -= Time.deltaTime;
            if (atkCountdown < 0) 
            {
                Attack(target);
                atkCountdown = atkPeriod;
                if (animator)
                    animator.SetTrigger("Run");
            }
        }
    }

    public void StopMoving() => navMeshAgent.isStopped = true;

    bool IsGoalInRange()
    {
        if (goal == null) return false;
        float dist2 = (goal.transform.position - transform.position).sqrMagnitude;
        return atkRangeSqr > dist2;
    }

    public override void Die(AttackInfo attackInfo)
    {
        GameController.INSTANCE?.OnEnemyDie(this);
        DoRagdoll(attackInfo);

        // Destroy gameObject after seconds
        Destroy(gameObject, 5f);
        Destroy(healthBarPivot, 1);

        // Destroy the Collider to invoke OnTriggerExit() event in the UnitRange object, so that the towers can pick another target
        Destroy(GetComponent<Collider>());
        Destroy(this);
    }

    private void DoRagdoll(AttackInfo attackInfo)
    {
        character.DoRagdoll(attackInfo);
    }

    public virtual void Attack(GameUnit gu)
    {
        // By default, just deal damage
        if (animator)
            animator.SetTrigger("Attack");
        AttackInfo attackInfo = new AttackInfo(gameObject, gu.gameObject, attackDamage);
        gu.TakeDmg(attackInfo);
    }

    public override void TakeDmg(AttackInfo attackInfo)
    {
        base.TakeDmg(attackInfo);
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

    public GameUnit GetTarget() => target;

    public void SetTarget(GameUnit gameUnit) => target = gameUnit;

    public int GetAttackDamage() => attackDamage;

    public void SetAtk(int a) => attackDamage = a;

    public float GetAtkRange() => atkRange;

    public void SetAtkRange(float a) => atkRange = a;
}
