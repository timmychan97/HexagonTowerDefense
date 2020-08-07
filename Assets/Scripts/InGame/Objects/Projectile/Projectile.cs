﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected GameObject emitter;
    protected Vector3 origin; // original position
    public bool follow; 
    public float speed; // move distance per second
    public Effect effect;
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Init(Unit _emitter, GameUnit _target) {}
    public virtual void Init(Enemy enemy, GameUnit _target) {}
    public virtual void OnHit() {}
}
