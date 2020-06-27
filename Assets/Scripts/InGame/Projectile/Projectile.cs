using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected GameObject emitter;
    protected Vector3 origin; // original position
    public bool follow; 
    public float speed; // move distance per second
    public int dmg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Init(Tower _emitter, GameObject _target)
    {
    }
    public virtual void Init(Tower _emitter, Enemy enemy){}
}
