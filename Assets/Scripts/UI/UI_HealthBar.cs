using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public HealthBarPivot pivot;

    public Slider slider;

    public bool forceHidden = false;

    // Transform position is updated by UI_HealthBarDisplayer for better performance.

    void Update()
    {
        // If the pivot is destroyed, destroy this game object
        if (!pivot)
            Destroy(gameObject);
        else
        {
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        var screenDestination = Camera.main.WorldToScreenPoint(pivot.transform.position);
        transform.position = screenDestination;
        var rectTransform = GetComponent<RectTransform>();

        if (screenDestination.z < 0 || !UI_Utils.IsVisibleOnScreen(rectTransform))
        {
            // The object is behind the canvas. Do not render.
            // Need to update the position too, because we need to know when it gets back to the screen
            Hide();
        }
        else
        {
            Show();
        }
    }



    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void Hide()
    {
        slider.gameObject.SetActive(false);
    }
    public void Show()
    {
        if (forceHidden)
            return;
        slider.gameObject.SetActive(true);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }


}
