using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRegion : MonoBehaviour
{
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    Effect effect;

    // Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        // collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    public void ApplyEffect(Effect effect) 
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null) 
            {
                enemy.TakeEffect(effect);
            }
        }
    }

    public void InflictDmg(int dmg)
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy != null) 
            {
                enemy.TakeDmg(dmg);
            }
        }
    }
    public void SetEffect(Effect _effect) 
    {
        effect = _effect;
    }
    public void SetPos(Vector3 pos) 
    {
        transform.position = pos;
    }
}
