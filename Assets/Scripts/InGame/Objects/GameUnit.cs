using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    public string _name;
    public string description;
    public int maxHp;
    public int cost;
    protected bool isDummy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescription()
    {
        return description;
    }

    public int getCost()
    {
        return cost;
    }

    public void SetIsDummy(bool b) 
    {
        isDummy = b;
    }
}
