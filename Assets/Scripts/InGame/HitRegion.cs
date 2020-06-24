using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRegion : MonoBehaviour
{
    SortedSet<Enemy> enemiesInRange = new SortedSet<Enemy>();

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

    void OnTriggerEnter(Collision other)
    {
        Debug.Log("Entere HitRegion");
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Add(enemy);
        }
    }

    void OnTriggerExit(Collision other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    public void InflictDmg(int dmg)
    {
        Debug.Log("Inflict Damage");
        Debug.Log(enemiesInRange.Count);
        foreach (Enemy enemy in enemiesInRange)
        {
            enemy.LoseHealth(dmg);
        }
        Destroy(gameObject);
    }

    public void SetPos(Vector3 pos) 
    {
        transform.position = pos;
    }
}
