using UnityEngine;

public class Enemy_Archer : Enemy
{
    public Projectile pf_projectile;
    public Transform emitter;

    protected new void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        Projectile p = Instantiate(pf_projectile, emitter.position, transform.rotation);
        p.Init(this, (GameUnit)attackTarget);
    }
}
