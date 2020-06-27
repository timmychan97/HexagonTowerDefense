using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IDamagable
{
    public int maxHp;
    private int hp;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        // display die animation
    }

    public void TakeDmg(float dmg)
    {
        hp -= Mathf.RoundToInt(dmg);
        if (hp <= 0)
        {
            Die();
        }
    }

    public int getHp() { return hp; }
}
