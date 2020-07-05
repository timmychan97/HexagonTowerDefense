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

    Vector3 GetPredictPos(Vector3 proj, Vector3 pos, Vector3 velocity)
    {
        // remove y component
        float t = ComputeDeltaT(proj, pos, velocity);
        return (pos + t * velocity);
    }

    float ComputeDeltaT(Vector3 _T, Vector3 _P, Vector3 _vt)
    {
        // _T: target current pos
        // _P: projectile current pos
        // _vt: velocity of target

        // remove y component，摊平到xz平面
        Vector3 T = new Vector3(_T.x, 0, _T.z);
        Vector3 P = new Vector3(_P.x, 0, _P.z);
        Vector3 vt = new Vector3(_vt.x, 0, _vt.y);
        Vector3 PT = T - P; 

        if (_vt == Vector3.zero) // exception
        {
            return PT.magnitude / speed;
        }

        // turn into second order polynomial, d = sqrt(b^2 - 4ac), (discriminant)
        float PT2 = PT.sqrMagnitude;            // = PT^2
        float PTLen = Mathf.Sqrt(PT2);
        float vtDotPT = Vector3.Dot(vt, PT);    // = v_t · PT
        float k = vt.magnitude / speed;         // k = |v_t| / speed
        float a = speed*speed*(1-k*k) / PTLen;  // a = (speed^2 * 1-k^2) / |PT|
        float b = - 2 * vtDotPT / PTLen;        // b = -2 (v_t · PT) / |PT|
        float c = - PTLen;                      // c = -|PT|
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
        hitRegion.InflictDmg(dmg);
        if (effect != null) {
            hitRegion.ApplyEffect(effect);
        }
        Destroy(hitRegion.gameObject);
    }
}
