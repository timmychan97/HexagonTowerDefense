using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : TileContent
{
    private int id;
    public int atk;
    public float atkSpeed; // in Hz
    private float atkPeriod; // in s
    public float lastAtkTime;
    public float maxHp;
    private float hp;
    public int cost;
    public float range;
    GameObject target = null;
    public Projectile projectile;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = range;
        sphereCollider.isTrigger = true;
        hp = maxHp;
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) ChooseNewTarget();

        if (target) {
            Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(lookAtPos);
            HandleAtk();
        }
    }

    public void HandleAtk()
    {
        float timeSinceAtk = Time.time - lastAtkTime;
        if (timeSinceAtk > atkPeriod) 
        {
            Atk(target);
            lastAtkTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Tower");
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy) // the collided object is an Enemy
        {
            if (!target) 
            {
                SetTarget(enemy.gameObject);
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
        if (enemy)
        {
            Debug.Log("OnTriggerExit: " + enemy.gameObject.ToString());
            enemiesInRange.Remove(enemy);
            if (enemy.gameObject == target) 
            {
                ChooseNewTarget();
            }
        }
    }

    void Atk(GameObject target) 
    {
        Projectile p = Instantiate(projectile, transform.position + Vector3.up, transform.rotation);
        p.Init(this, target);
    }

    void SetTarget(GameObject _target) 
    {
        target = _target;
        Debug.Log("Set Target");
    }

    // returns try when successfully chosen a new target
    bool ChooseNewTarget()
    {
        Debug.Log("Choose new target");
        Debug.Log(enemiesInRange.Count);
        if (enemiesInRange.Count == 0) return false;  // no enemies in range
        Debug.Log(enemiesInRange.Count.ToString());
        
        foreach (Enemy e in enemiesInRange)
        {
            if (e == null) 
            {
                enemiesInRange.Remove(e);
            }
            else 
            {
                target = e.gameObject;
                enemiesInRange.Remove(e);
                break;
            }
        }
        return true;
    }

    void HandleTargetDead() {

    }
}
