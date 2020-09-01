using UnityEngine;

public class AttackInfo
{
    //public IAttackable attacker;
    //public IDamagable target;
    //public float damage;
    //public Projectile projectile;

    //public string additionalInfo;

    //private void Create(IAttackable attacker, IDamagable target, float damage, Projectile projectile)
    //{
    //    this.attacker = attacker;
    //    this.target = target;
    //    this.damage = damage;
    //    this.projectile = projectile;
    //}
    //public AttackInfo(IAttackable attacker, IDamagable target, float damage)
    //{
    //    Create(attacker, target, damage, null);
    //}

    //public AttackInfo(IAttackable attacker, IDamagable target, float damage, Projectile projectile)
    //{
    //    Create(attacker, target, damage, projectile);
    //}

    public GameObject attacker;
    public GameObject target;
    public float damage;
    public Projectile projectile;

    public string additionalInfo;

    private void Create(GameObject attacker, GameObject target, float damage, Projectile projectile)
    {
        this.attacker = attacker;
        this.target = target;
        this.damage = damage;
        this.projectile = projectile;
    }
    public AttackInfo(GameObject attacker, GameObject target, float damage)
    {
        Create(attacker, target, damage, null);
    }

    public AttackInfo(GameObject attacker, GameObject target, float damage, Projectile projectile)
    {
        Create(attacker, target, damage, projectile);
    }
}
