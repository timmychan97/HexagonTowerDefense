using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    public Projectile projectile;
    public Transform emitter;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    public override void Atk(GameUnit gu)
    {
        Projectile p = Instantiate(projectile, emitter.position, transform.rotation);
        p.Init(this, target);
    }
}
