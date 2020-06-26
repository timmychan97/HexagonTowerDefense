using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class UI_HealthBarDisplayer : MonoBehaviour
{
    public static UI_HealthBarDisplayer INSTANCE;
    public UI_HealthBar healthBarPf;

    private List<UI_HealthBar> healthBars = new List<UI_HealthBar>();

    void Awake()
    {
        INSTANCE = this;
    }

    void Update()
    {
        // Iterate list in reverse to support item removal.
        for (int i = healthBars.Count - 1; i >= 0; i--)
        {
            var healthBar = healthBars[i];
            var pivot = healthBar.pivot;

            // if the object is destroyed
            if (!pivot)
            {
                RemoveHealthBar(healthBar);
            }
            else
            {
                UpdateUI(healthBar);
            }
        }

    }


    /* For a UI_HealthBar, update its position and healthbar values.
     * 
     * This method is put here and not in UI_HealthBar because this method need to set the game object to inactive, which stops a script from running.
     */
    void UpdateUI(UI_HealthBar healthBar)
    {
        var screenDestination = Camera.main.WorldToScreenPoint(healthBar.pivot.transform.position);
        healthBar.transform.position = screenDestination;
        if (screenDestination.z < 0 || !IsVisibleOnScreen(healthBar))
        {
            // The object is behind the canvas. Do not render.
            healthBar.gameObject.SetActive(false);
            // Need to update the position too, because we need to know when it gets back to the screen
        }
        else
        {
            healthBar.gameObject.SetActive(true);
        }
    }



    public void AddHealthBar(HealthBarPivot pivot)
    {
        var healthBar = Instantiate(healthBarPf, transform);
        healthBar.pivot = pivot;
        healthBars.Add(healthBar);
    }

    // Nice to have if we want to hide a healthbar without pivot being null
    public void RemoveHealthBar(HealthBarPivot pivot)
    {
        foreach(var healthBar in healthBars)
        {
            if (healthBar.pivot == pivot)
            {
                RemoveHealthBar(healthBar);
                break;
            }
        }
    }

    private void RemoveHealthBar(UI_HealthBar healthBar)
    {
        healthBars.Remove(healthBar);
        Destroy(healthBar.gameObject);
    }



    // Checks if a UI element is visible at all on screen. Return true if at least one pixel of UI is visible.
    bool IsVisibleOnScreen(UI_HealthBar ui)
    {
        Vector3[] v = new Vector3[4];
        ui.GetComponent<RectTransform>().GetWorldCorners(v);

        float maxX = Mathf.Max(v[0].x, v[1].x, v[2].x, v[3].x);
        if (maxX < 0) return false;

        float minX = Mathf.Min(v[0].x, v[1].x, v[2].x, v[3].x);
        if (minX > Screen.width) return false;

        float maxY = Mathf.Max(v[0].y, v[1].y, v[2].y, v[3].y);
        if (maxY < 0) return false;

        float minY = Mathf.Min(v[0].y, v[1].y, v[2].y, v[3].y);
        if (minY > Screen.height) return false;

        return true;
    }
}
