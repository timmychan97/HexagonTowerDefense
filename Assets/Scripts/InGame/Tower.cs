using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : TileContent, IDamagable
{
    private int id;
    public int atk;
    public Effect effect;
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
    SphereCollider sphereCollider;
    public GameObject platform;
    public GameObject towerContent;
    public GameObject emitter;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null) 
        {
            sphereCollider = gameObject.AddComponent<SphereCollider>();
        }
        sphereCollider.isTrigger = true;
        sphereCollider.radius = range;
        hp = maxHp;
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) ChooseNewTarget();

        if (target) 
        {
            HandleAtk();
        }
    }

    public void HandleAtk()
    {
        float timeSinceAtk = Time.time - lastAtkTime;
        if (timeSinceAtk > atkPeriod) 
        {
            // rotate tower content to look at target
            if (towerContent != null) 
            {
                Vector3 lookAtPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                towerContent.transform.LookAt(lookAtPos);
            }
            Atk(target);
            lastAtkTime = Time.time;
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
            if (enemy.gameObject == target) 
            {
                target = null;
                ChooseNewTarget();
            }
        }
    }

    void Atk(GameObject target) 
    {
        Projectile p = Instantiate(projectile, emitter.transform.position, transform.rotation);
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null) 
        {
            p.Init(this, enemy);
        }
        else
        {
            p.Init(this, target);
        }
    }

    void SetTarget(GameObject _target) 
    {
        target = _target;
    }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
    }

    // returns try when successfully chosen a new target
    bool ChooseNewTarget()
    {
        if (enemiesInRange.Count == 0) return false;  // no enemies in range
        List<Enemy> toRemove = new List<Enemy>();
        foreach (Enemy e in enemiesInRange)
        {
            toRemove.Add(e); // save the enemies that turned into null while in range
            if (e != null)
            {
                target = e.gameObject;
                break;
            }
        }

        // remove the null's in enemiesInRange
        foreach (Enemy e in toRemove)
        {
            enemiesInRange.Remove(e);
        }
        return true;
    }
}
