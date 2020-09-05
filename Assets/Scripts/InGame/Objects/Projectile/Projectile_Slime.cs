using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Slime : Projectile_FixedTarget
{
    public GameObject slime;

    void Update()
    {
        UpdatePos();
    }

    public override void OnHit()
    {
        base.OnHit();
        Instantiate(slime, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
