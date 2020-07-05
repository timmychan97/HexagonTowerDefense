using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRegion : MonoBehaviour
{
    static int numEffect; // used to check if enemy is already affected by this effect (slow)
    public int effectId;
    public float lifespan;
    float countdown;
    public Effect slimeSlowEffect;
    HashSet<Enemy> enemiesInRange = new HashSet<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        countdown = lifespan;
        effectId = numEffect++;
        slimeSlowEffect.id = effectId; 
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown < 0) 
        {
            Destroy(gameObject);
        } 
        else 
        {
            HandleEffect();
        }
    }
    void HandleEffect()
    {
        if (slimeSlowEffect == null) 
        {
            Debug.LogWarning("No slow effect is applied to slime region");
        }
        ApplyEffect(slimeSlowEffect);
    }
    void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }
    void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.transform.GetComponent<Enemy>();
        if (enemy != null)
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
    public void SetPos(Vector3 pos) 
    {
        transform.position = pos;
    }
}
