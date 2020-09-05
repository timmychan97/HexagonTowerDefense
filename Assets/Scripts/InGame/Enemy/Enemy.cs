using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Enemy : AttackableGameUnit, IAffectable
{
    public HealthBarPivot healthBarPivot;
    public GameUnit goal;

    /* stats */
    public int level;
    public float moveSpeed;
    public int worth;

    protected NavMeshAgent navMeshAgent;
    public HashSet<Effect> effects = new HashSet<Effect>();

    protected override void Start()
    {
        base.Start();
        ValidateAttachedObjects();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        if (navMeshAgent && goal)
            navMeshAgent.destination = goal.transform.position;

        hp = maxHp;

        if (healthBarPivot) healthBarPivot.AddUIHealthBar(maxHp);
    }

    protected override void SubUpdate()
    {
        base.SubUpdate();

        //if (Input.GetMouseButton(0))
        //{
        //    int cameraDistance = 600;
        //    RaycastHit rh;
        //    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(r, out rh, cameraDistance))
        //    {
        //        navMeshAgent.destination = rh.point;
        //    }
        //}

        UpdateMovement();
    }

    protected void UpdateMovement()
    {
        // When doing ragdoll, the system will disable the agent. Make sure we check if it is enabled before moving
        if (navMeshAgent.enabled)
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }
    }

    protected virtual void Move(Vector3 destination) // in local space
    {
        // If not explicitely disabled, the navmesh will move on its own, without calling move
    }


    /// <summary>
    /// This methods outputs warnings to the developers providing info 
    /// about what is not properly set up in inspector for this enemy
    /// </summary>
    protected virtual void ValidateAttachedObjects()
    {
        if (!healthBarPivot) Debug.LogWarning($"No HealthBarPivot is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector. No health will be shown for this enemy");
        if (!goal) Debug.LogWarning($"No goal transform is attached to \"{gameObject.name}\" " +
            $"Enemy in inspector");
    }

    public override void HandleAttack()
    {
        if (IsGoalInRange())
        {
            Debug.Log("Goal is in range");
            StopMoving();
            if (isReadyToAttack())
            {
                SetAttackTarget(goal);
                Attack();
            }
            // attack goal (base)
        }
        else if (attackTarget != null && !attackTarget.IsDead())
        {
            // Visualize target
#if UNITY_EDITOR
            if (((GameUnit)attackTarget))
                Debug.DrawLine(transform.position + (Vector3.up * 1f), ((GameUnit)attackTarget).transform.position, Color.red);
#endif
            // If this enemy can attack units
            if (isReadyToAttack()) 
            {
                Attack();
            }
        }
    }

    public void StopMoving() => navMeshAgent.isStopped = true;

    bool IsGoalInRange()
    {
        if (goal == null) return false;
        float dist2 = (goal.transform.position - transform.position).sqrMagnitude;
        return attackRangeSqr > dist2;
    }

    public override void Die(AttackInfo attackInfo)
    {
        GameController.INSTANCE?.OnEnemyDie(this);

        PlayDieAnimation(attackInfo);

        // Destroy gameObject after seconds
        Destroy(gameObject, 5f);
        Destroy(healthBarPivot, 1);

        if (GetComponent<Collider>())
            Destroy(GetComponent<Collider>());
        if (range && range.gameObject)
            Destroy(range.gameObject);

        // Destroy this script to invoke OnTriggerExit() event in the UnitRange object, so that the towers can pick another target
        Destroy(this);
    }

    protected virtual void PlayDieAnimation(AttackInfo attackInfo)
    {
        // OVERRIDE THIS
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void TakeDmg(AttackInfo attackInfo)
    {
        if (IsDead()) return;
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

    public override UI_PanelUnitInfo GetPanelUnitInfo() 
    {
        // get the prefab for Enemy class from UI_PanelUnitInfoManager, 
        // link it with this object, then return the panel
        UI_PanelUnitInfo_Enemy panel = UI_PanelUnitInfoManager.INSTANCE.pf_enemyPanel;
        panel.SetEnemy(this);
        return panel; 
    }
}
