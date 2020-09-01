using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableGameUnit : GameUnit, IAttackable
{
    public float attackRange;
    public float attackDamage;
    public float attackFrequency; // in Hz

    float attackRangeSqr;
    float attackPeriod;
    float attackCountdown;

    private IDamagable target;

    protected virtual void Start()
    {
        attackRangeSqr = attackRange * attackRange;
        attackPeriod = 1f / attackFrequency;
        attackCountdown = 0;
    }


    public void SetTarget(IDamagable _target) => target = _target;

    public float GetAttackDamage() => attackDamage;

    public void SetAttackDamage(float value) => attackDamage = value;

    public float GetAttackRange() => attackRange;

    public void SetAttackRange(float value) => attackRange = value;

    public virtual void Attack(IDamagable target)
    {
        // By default, just deal damage
        //AttackInfo attackInfo = new AttackInfo(this, target, attackDamage);
        //target.TakeDmg(attackInfo);
    }
}
