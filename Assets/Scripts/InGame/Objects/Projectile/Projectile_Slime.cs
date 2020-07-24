using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Slime : Projectile_FixedTarget
{
    public GameObject slime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePos();
    }

    public override void OnHit()
    {
        base.OnHit();
        Instantiate(slime, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
