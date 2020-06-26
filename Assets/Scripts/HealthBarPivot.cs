using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPivot : MonoBehaviour
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }

    void Start()
    {
        UI_HealthBarDisplayer.INSTANCE.AddHealthBar(this);
    }


    public void RemoveUIHealthBar()
    {
        UI_HealthBarDisplayer.INSTANCE.RemoveHealthBar(this);
    }
}
