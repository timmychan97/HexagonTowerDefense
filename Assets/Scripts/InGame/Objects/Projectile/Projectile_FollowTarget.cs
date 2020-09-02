using UnityEngine;

public class Projectile_FollowTarget : Projectile
{
    IAttackable attacker;
    GameUnit target;

    void Update()
    {
        if (target == null)  // disappears when the target object is destroyed
        {
            Destroy(gameObject);
        }
        else 
        {
            UpdatePos();
        }
    }

    public void Init(float _dmg, GameUnit _target)
    {
        damage = _dmg;
        target = _target;
        transform.LookAt(_target.transform, Vector3.up);
    }

    public override void Init(Unit _emitter, GameUnit _target)
    {
        effect = _emitter.effect;
        Init(_emitter.attackDamage, _target);
    }

    public override void Init(Enemy enemy, GameUnit unit)
    {
        attacker = enemy;
        Init(enemy.attackDamage, unit);
    }

    void UpdatePos() 
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float dist = toTarget.magnitude;
        if (dist < speed * Time.deltaTime) // will reach target on next frame
        {
            OnHit();
            Destroy(gameObject);
        } 
        else 
        {
            transform.position += toTarget.normalized * speed * Time.deltaTime;
        }
    }
    
    public override void OnHit()
    {
        AttackInfo attackInfo = new AttackInfo(attacker, target, damage, this);
        target.TakeDmg(attackInfo);
        if (effect != null)
        {
            target.TakeEffect(effect);
        }
    }
}
