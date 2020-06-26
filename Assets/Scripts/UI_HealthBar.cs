using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public HealthBarPivot pivot;

    public Slider slider;

    void Update()
    {
        if (pivot)
        {
            slider.maxValue = pivot.MaxHealth;
            slider.value = pivot.Health;
        }
    }

    void OnHealthChange(float healthChange)
    {
        //slider.maxValue =
        // Support displaying a fancy animation when a 
    }

    //void UpdateHealth()


    

}
