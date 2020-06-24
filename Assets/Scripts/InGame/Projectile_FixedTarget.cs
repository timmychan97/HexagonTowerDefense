using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FixedTarget : Projectile
{
    Vector3 target;
    public HitRegion hitRegionPf;
    private HitRegion hitRegion;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
    }

    public override void Init(Tower _emitter, GameObject _target)
    {
        target = _target.transform.position;
        dmg = _emitter.atk;

        // set hit region
        hitRegion = Instantiate(hitRegionPf, target, transform.rotation);
    }

    void UpdatePos()
    {
        Vector3 toTarget = target - transform.position;
        if (toTarget.magnitude < speed * Time.deltaTime) // will reach on next frame
        {
            InflictDmg();
            Destroy(gameObject);
        }
        else 
        {
            transform.position += toTarget.normalized * speed * Time.deltaTime;
        }
    }

    public void InflictDmg()
    {
        hitRegion.InflictDmg(dmg);
    }
}
