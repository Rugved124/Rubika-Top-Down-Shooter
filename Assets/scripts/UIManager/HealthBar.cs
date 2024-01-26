using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = PCHealth.instance.maxHitPoints;
        slider.value = slider.maxValue;
    }
    void Update()
    {
        slider.value = PCHealth.instance.hitPoints;
    }
}
