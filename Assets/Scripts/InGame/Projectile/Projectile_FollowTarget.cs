using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FollowTarget : Projectile
{
    GameObject target;
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

    public override void Init(Tower _emitter, GameObject _target)
    {
        target = _target;
        dmg = _emitter.atk;
        transform.LookAt(_target.transform, Vector3.up);
    }

    public override void Init(Tower _emitter, Enemy enemy)
    {
        Init(_emitter, enemy.gameObject);
    }

    void UpdatePos() 
    {
        Vector3 toTarget = target.transform.position - transform.position;
        float dist = toTarget.magnitude;
        if (dist < speed * Time.deltaTime) // will reach target on next frame
        {
            InflictDmg(target);
            
            Destroy(gameObject);
        } 
        else 
        {
            transform.position += toTarget.normalized * speed * Time.deltaTime;
        }
    }
    
    protected void InflictDmg(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.TakeDmg(dmg);
    }
}
