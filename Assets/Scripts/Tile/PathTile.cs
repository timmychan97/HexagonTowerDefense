using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : Tile
{
    public float slowDown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Nice");
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.black);
        }
    }
}
