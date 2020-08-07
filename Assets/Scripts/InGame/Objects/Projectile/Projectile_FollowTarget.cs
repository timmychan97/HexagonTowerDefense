using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FollowTarget : Projectile
{
    GameUnit target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void Init(int _dmg, GameUnit _target)
    {
        dmg = _dmg;
        target = _target;
        transform.LookAt(_target.transform, Vector3.up);
    }

    public override void Init(Unit _emitter, GameUnit _target)
    {
        effect = _emitter.effect;
        Init(_emitter.atk, _target);
    }

    public override void Init(Enemy enemy, GameUnit unit)
    {
        Init(enemy.atk, unit);
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
        target.TakeDmg(dmg);
        if (effect != null)
        {
            target.TakeEffect(effect);
        }
    }
}
