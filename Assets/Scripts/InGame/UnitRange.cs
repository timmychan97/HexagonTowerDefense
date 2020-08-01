using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    Unit unit;
    SphereCollider sphereCollider;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        // sphereCollider = GetComponent<SphereCollider>();
        // if (sphereCollider == null) 
        // {
        //     sphereCollider = gameObject.AddComponent<SphereCollider>();
        // }
        // sphereCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Unit _t) 
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null) 
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true;
        SetTower(_t);
        SetRadius(_t.range);
    }

    public void SetTower(Unit t)
    {
        unit = t;
    }
    public float GetRadius()
    {
        return sphereCollider.radius;
    }

    public void SetRadius(float r)
    {
        sphereCollider.radius = r;
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy) // the collided object is an Enemy
        {
            if (unit.GetTarget() == null) 
            {
                unit.SetTarget(enemy.gameObject);
            }
            else
            {
                enemiesInRange.Add(enemy); // backup enemies
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
            if (enemy.gameObject == unit.GetTarget()) 
            {
                unit.SetTarget(GetNewTarget());
            }
        }
    }

    
    public GameObject GetNewTarget()
    {
        /*
        chooses a new target by following policy:
        - the firstmost enemy that entered tower's range
        
        RETURN: true when successfully chosen a new target
        */

        GameObject target = null;
        // store nulls in the list of enemies 
        // (a result of enemies dying without leaving the range)
        List<Enemy> toRemove = new List<Enemy>();
        foreach (Enemy e in enemiesInRange)
        {
            // mark for removal afterwards 
            // (can't remove during foreach loop of hashset)
            toRemove.Add(e);  
            if (e != null)
            {
                target = e.gameObject; // found new target
                break;
            }
        }

        // remove the null's in enemiesInRange
        foreach (Enemy e in toRemove)
        {
            enemiesInRange.Remove(e);
        }
        return target;
    }
}
