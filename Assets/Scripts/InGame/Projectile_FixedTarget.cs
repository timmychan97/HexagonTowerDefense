using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FixedTarget : Projectile
{
    public HitRegion hitRegionPf;
    private HitRegion hitRegion;
    public float downAccel = -9.8f;
    private Vector3 orig;
    private Vector3 dir;   // direction on xz plane
    private Vector3 target;
    private float t0;
    private float y0;
    private float vy; // initial y velocity
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
        orig = transform.position;
        target = _target.transform.position;
        dir = (target - orig);
        dir.y = 0;
        dir = dir.normalized;
        dmg = _emitter.atk;

        // set hit region
        hitRegion = Instantiate(hitRegionPf, target, transform.rotation);

        // compute initial velocity in y axis
        t0 = Time.time;
        y0 = transform.position.y;
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0;
        float deltaT = toTarget.magnitude / speed;
        float deltaY = target.y - transform.position.y;
        vy = (deltaY - 0.5f * downAccel * deltaT * deltaT) / deltaT;
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
            // calculate y coordinate using formula for constant acceleration
            float t = Time.time - t0;
            float y = vy * t + 0.5f * downAccel * t * t; // relative to y0
            transform.position = orig + dir * speed * t + y * Vector3.up; // move along xz plane
        }
    }

    public void InflictDmg()
    {
        hitRegion.InflictDmg(dmg);
    }
}
