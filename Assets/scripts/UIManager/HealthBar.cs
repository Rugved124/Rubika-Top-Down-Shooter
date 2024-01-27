using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : PCStats
{

    Slider slider;
    public Image redbar;
    public Color poisonedcolor;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxHitPoints;
        slider.value = slider.maxValue;
    }
    void Update()
    {
        if (PCStatusEffects.instance.isPoisoned) redbar.color = poisonedcolor;
        else redbar.color = Color.red;
        slider.value = PCHealth.instance.GetHitPoints();
    }
}
