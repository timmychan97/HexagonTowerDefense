using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableGameUnit : GameUnit, IAttackable
{
    public float attackRadius;
    public float attackDamage;
    public float attackFrequency; // in Hz

    protected float attackRangeSqr;
    float attackPeriod;
    protected float attackCountdown = 0f;

    [SerializeField] protected IDamagable attackTarget;

    public Range pf_range;
    private Range range;

    protected virtual void Start()
    {
        attackRangeSqr = attackRadius * attackRadius;
        attackPeriod = 1f / attackFrequency;

        range = Instantiate(pf_range, transform);
        if (!range) Debug.LogWarning("AttackableGameUnit has no Range script attached to it");

        range.Init(this, attackRadius);
    }

    protected override void SubUpdate()
    {
        UpdateAttackCountdown();
        HandleAttack();
    }

    protected void UpdateAttackCountdown()
    {
        if (!isReadyToAttack())
        {
            attackCountdown -= Time.deltaTime;
        }
    }

    public virtual void HandleAttack()
    {
        Debug.LogWarning("Called HandleAttack() in AttackableGameUnit. Make sure you override this method!");
    }

    public void SetAttackTarget(IDamagable attackTarget) => this.attackTarget = attackTarget;
    public float GetAttackDamage() => attackDamage;
    public void SetAttackDamage(float value) => attackDamage = value;
    public float GetAttackRadius() => attackRadius;
    public void SetAttackRadius(float value) => attackRadius = value;

    public virtual void Attack()
    {
        ResetAttackCountdown();
        // By default, just deal damage
        AttackInfo attackInfo = new AttackInfo(this, attackTarget, attackDamage);
        attackTarget.TakeDmg(attackInfo);
    }

    protected void ResetAttackCountdown()
    {
        attackCountdown = attackPeriod;
    }
    protected bool isReadyToAttack()
    {
        return attackCountdown <= 0;
    }
}
