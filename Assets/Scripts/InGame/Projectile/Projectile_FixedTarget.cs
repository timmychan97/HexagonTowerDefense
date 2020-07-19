using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FixedTarget : Projectile
{
    public ParticleEffect particleEffect;

    public HitRegion hitRegionPf;
    private HitRegion hitRegion;
    public float downAccel = -9.8f;
    private Vector3 orig;
    private Vector3 dir;   // direction on xz plane
    protected Vector3 target;
    private float t0;
    private float deltaT;
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
        deltaT = toTarget.magnitude / speed;
        float deltaY = target.y - transform.position.y;
        vy = (deltaY - 0.5f * downAccel * deltaT * deltaT) / deltaT;
    }

    public override void Init(Tower _emitter, Enemy enemy)
    {
        dmg = _emitter.atk;
        orig = transform.position;
        target = GetPredictPos(orig, enemy.transform.position, enemy.GetVelocity());   // shoot in the direction target is moving towards
        // set hit region
        hitRegion = Instantiate(hitRegionPf, target, transform.rotation);


        dir = target - orig;
        dir.y = 0;
        dir = dir.normalized;


        // compute initial velocity in y axis
        t0 = Time.time;
        y0 = transform.position.y;
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0;
        deltaT = toTarget.magnitude / speed;
        float deltaY = target.y - transform.position.y;
        vy = (deltaY - 0.5f * downAccel * deltaT * deltaT) / deltaT;
    }

    Vector3 GetPredictPos(Vector3 projPos, Vector3 targetPos, Vector3 targetVelocity)
    {
        Debug.Log($"GetPredictPos({projPos}, {targetPos}, {targetVelocity}");
        // remove y component
        float t = ComputeDeltaT(projPos, targetPos, targetVelocity);
        Debug.Log($"t = {t}");
        return (targetPos + t * targetVelocity);
    }

    float ComputeDeltaT(Vector3 _P, Vector3 _T, Vector3 _vt)
    {
        // _T: target current pos
        // _P: projectile current pos
        // _vt: velocity of target
        Debug.Log($"ComputeDeltaT({_P}, {_T}, {_vt})");

        // remove y component，摊平到xz平面
        Vector3 T = new Vector3(_T.x, 0, _T.z);
        Vector3 P = new Vector3(_P.x, 0, _P.z);
        Vector3 vt = new Vector3(_vt.x, 0, _vt.z);
        Vector3 TP = P - T; 
        float TP2 = TP.sqrMagnitude;      // = |TP|^2
        float vt2 = vt.sqrMagnitude;      // = |v_t|^2
        float vtLen = Mathf.Sqrt(vt2);    // = |v_t|

        // Exceptions: 
        // if target is idle or faster than projectile, 
        // shoot at target's current position
        if (_vt == Vector3.zero) 
        {
            return TP.magnitude / speed;
        } 
        if (vtLen >= speed)
        {
            return 0.0f;
        }

        // turn into second order polynomial, d = sqrt(b^2 - 4ac), (discriminant)
        float a = speed*speed-vt2;              // a = (speed^2 * |v_t|^2)
        float b = 2 * Vector3.Dot(TP, vt);      // b = 2 (TP · v_t)
        float c = -TP2;                         // c = -|TP|^2
        float d = Mathf.Sqrt(b*b - 4 * a * c);  // d = discriminant
        float t =  (-b + d) / (2 * a);
        return t;
    }

    protected void UpdatePos()
    {
        float t = Time.time - t0;
        if (t > deltaT) // reached target
        {
            OnHit();
            Destroy(gameObject);
        }
        else 
        {
            // calculate y coordinate using formula for constant acceleration
            float y = vy * t + 0.5f * downAccel * t * t; // relative to y0
            transform.position = orig + dir * speed * t + y * Vector3.up; // move along xz plane
        }
    }

    public override void OnHit()
    {
        var pfx = Instantiate(particleEffect.gameObject);
        pfx.transform.position = transform.position;

        hitRegion.InflictDmg(dmg);
        if (effect != null) {
            hitRegion.ApplyEffect(effect);
        }
        Destroy(hitRegion.gameObject);
    }
}
