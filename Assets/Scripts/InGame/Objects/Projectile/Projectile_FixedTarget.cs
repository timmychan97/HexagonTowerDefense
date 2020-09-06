using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FixedTarget : Projectile
{
    private IAttackable attacker;

    public ParticleEffect particleEffect;

    public float downAccel = -9.8f;
    private Vector3 orig;
    private Vector3 dir;  // Direction on xz plane
    protected Vector3 target;
    private float t0;
    private float deltaT;
    private float vy;  // Initial y velocity

    public float hitRadius = 1f;
    public float explosionForce = 1f;

    void Update() => UpdatePos();

    public override void Init(Unit _emitter, GameUnit _target)
    {
        attacker = _emitter;
        damage = _emitter.attackDamage; 
        orig = transform.position;

        if (_target is Enemy enemy) 
        {
            target = GetPredictPos(orig, enemy.transform.position, enemy.GetVelocity());   // shoot in the direction target is moving towards
            ToGnd(ref target);
        }
        else
        {
            // NOTE: this assumes that _target is idle
            target = _target.transform.position;
        }

        dir = (target - orig);
        dir.y = 0;
        dir = dir.normalized;

        ComputeInitialVelocityY();
    }

    void ComputeInitialVelocityY()
    {
        // Compute initial velocity in y axis
        t0 = Time.time;
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0;
        deltaT = toTarget.magnitude / speed;
        float deltaY = target.y - transform.position.y;
        vy = (deltaY - 0.5f * downAccel * deltaT * deltaT) / deltaT;
    }

    Vector3 GetPredictPos(Vector3 projPos, Vector3 targetPos, Vector3 targetVelocity)
    {
        // Remove y component
        float t = ComputeDeltaT(projPos, targetPos, targetVelocity);
        return (targetPos + t * targetVelocity);
    }

    float ComputeDeltaT(Vector3 _P, Vector3 _T, Vector3 _vt)
    {
        // _T: target current pos
        // _P: projectile current pos
        // _vt: velocity of target

        // remove y component，摊平到xz平面
        Vector3 T = new Vector3(_T.x, 0, _T.z);
        Vector3 P = new Vector3(_P.x, 0, _P.z);
        Vector3 vt = new Vector3(_vt.x, 0, _vt.z);
        Vector3 TP = P - T; 
        float TP2 = TP.sqrMagnitude;      // = |TP|^2
        float vt2 = vt.sqrMagnitude;      // = |v_t|^2
        float vtLen = Mathf.Sqrt(vt2);    // = |v_t|

        // Exceptions: 
        // If target is idle or faster than projectile, 
        // shoot at target's current position
        if (_vt == Vector3.zero) 
        {
            return TP.magnitude / speed;
        } 
        if (vtLen >= speed)
        {
            return 0.0f;
        }

        // Turn into second order polynomial, d = sqrt(b^2 - 4ac), (discriminant)
        float a = speed*speed-vt2;              // a = (speed^2 * |v_t|^2)
        float b = 2 * Vector3.Dot(TP, vt);      // b = 2 (TP · v_t)
        float c = -TP2;                         // c = -|TP|^2
        float d = Mathf.Sqrt(b*b - 4 * a * c);  // d = discriminant
        float t =  (-b + d) / (2 * a);
        return t;
    }

    Vector3 ToGnd(ref Vector3 pos) 
    {
        // Return 
        Vector3 above = pos + Vector3.up * 100;
        RaycastHit hit;
        Ray ray = new Ray(above, Vector3.down);
        if (Physics.Raycast(ray, out hit, 200, TileUtils.MAP_LAYER_MASK))
        {
			Transform objectHit = hit.transform;
			Tile tile = objectHit.gameObject.GetComponentInParent<Tile>();
            if (tile != null) 
            {
                pos += Vector3.up * (tile.GetY() - pos.y); // Changes it
            }

        }
        return pos;
    }

    protected void UpdatePos()
    {
        float t = Time.time - t0;
        if (t > deltaT) // Reached target
        {
            OnHit();
            Destroy(gameObject);
        }
        else 
        {
            // Calculate y coordinate using formula for constant acceleration
            float y = vy * t + 0.5f * downAccel * t * t; // Relative to y0
            transform.position = orig + dir * speed * t + y * Vector3.up; // Move along xz plane
        }
    }

    public override void OnHit()
    {
        // If there is a particle effect assigned. The effect will be instantiated
        if (particleEffect)
        {
            var pfx = Instantiate(particleEffect.gameObject);
            pfx.transform.position = transform.position;
            CameraShaker.ExplosionShake(transform.position);
        }

        var enemiesInRange = GetEnemiesWithinRadius();

        foreach (Enemy enemy in enemiesInRange)
        {
            // Calculate the damage to apply upon the enemy by distance. With a linear falloff of damage amount
            var proximity = (transform.position - enemy.transform.position).magnitude;
            var damageMultiplier = Mathf.Max(0, 1 - (proximity / hitRadius));
            var explosionDamage = damage * damageMultiplier;

            // Attack it
            AttackInfo attackInfo = new AttackInfo(attacker, enemy, explosionDamage, this);
            enemy.TakeDmg(attackInfo);

            // Apply effects
            if (effect != null)
            {
                enemy.TakeEffect(effect);
            }
        }
    }

    /// <summary>
    /// Return all enemies within hitRadius
    /// </summary>
    public HashSet<Enemy> GetEnemiesWithinRadius()
    {
        var enemies = new HashSet<Enemy>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitRadius);
        foreach (Collider col in colliders)
        {
            var enemy = col.GetComponent<Enemy>();
            if (enemy)
                enemies.Add(enemy);
        }
        return enemies;
    }
}
