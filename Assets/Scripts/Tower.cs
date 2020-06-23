using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : TileContent
{
    private int id;
    public float atk;
    public float atkSpeed; // in Hz
    private float atkPeriod; // in s
    public float lastAtkTime;
    public float maxHp;
    private float hp;
    public int cost;
    public float range;
    GameObject target;
    public Projectile projectile;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        hp = maxHp;
        atkPeriod = 1.0f / atkSpeed;
        lastAtkTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (ChooseATarget()) 
        {
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
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy) 
        {
            enemiesInRange.Add(enemy);
            SetTarget(enemy.gameObject);
        }
    }

    void Atk(GameObject target) 
    {
        Projectile p = Instantiate(projectile, transform.position, transform.rotation);
        p.SetTarget(target);
    }

    void SetTarget(GameObject _target) 
    {
        target = _target;
    }

    // returns true when successfully chose a target
    bool ChooseATarget()
    {
        if (target) return true; // already has a target
        if (enemiesInRange.Count == 0) return false;  // no enemies in range

        return true;
    }
}
