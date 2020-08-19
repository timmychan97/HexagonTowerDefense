using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPivot : MonoBehaviour
{
    private UI_HealthBar healthBar;

    /// <summary>
    /// Add HealthBar on the UI
    /// </summary>
    public void AddUIHealthBar() => healthBar = UI_HealthBarManager.INSTANCE.AddHealthBar(this);

    /// <summary>
    /// Add HealthBar on the UI and set both health and max health to the given value
    /// </summary>
    public void AddUIHealthBar(float health)
    {
        AddUIHealthBar();
        SetHealth(health);
        SetMaxHealth(health);
    }

    public void RemoveUIHealthBar() => healthBar.Remove();

    public void SetHealth(float health) => healthBar.SetHealth(health);

    public void SetMaxHealth(float maxHealth) => healthBar.SetMaxHealth(maxHealth);


    // Might be helpful when doing Settings
    public void HideUIHealthBar()
    {
        healthBar.forceHidden = true;
        healthBar.Hide();
    }
    public void ShowUIHealthBar()
    {
        healthBar.forceHidden = false;
        healthBar.Show();
    }
}
