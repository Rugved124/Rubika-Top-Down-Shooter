using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstBullet : PCStats
{

    Slider slider;
    public Image redbar;
    public Color poisonedcolor;
    private BulletType type;
    void Start()
    {
        slider = GetComponent<Slider>();
        type = GetComponent<BulletType>();
        slider.maxValue = type.GiveFirstBulletCount();
        slider.value = slider.maxValue;
    }
    void Update()
    {
        slider.maxValue = type.GiveFirstBulletCount();
        slider.value = type.GetFirstbulletCount();
        //if(type.GetFirstBullet() == type.bulletType.Red)
       // {

        //}
    }
}
