using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class UI_HealthBarManager : MonoBehaviour
{
    public static UI_HealthBarManager INSTANCE;
    public UI_HealthBar healthBarPf;

    void Awake() => INSTANCE = this;

    public UI_HealthBar AddHealthBar(HealthBarPivot pivot)
    {
        var healthBar = Instantiate(healthBarPf, transform);
        healthBar.pivot = pivot;
        healthBar.UpdatePosition();
        return healthBar;
    }
}
