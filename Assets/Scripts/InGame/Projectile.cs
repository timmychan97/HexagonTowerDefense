using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    public float speed; // move distance per second
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        {
            Destroy(gameObject);
        }
        else 
        {
            UpdatePos();
        }
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

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public void InflictDmg(GameObject obj)
    {
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.LoseHealth(dmg);
    }
}
