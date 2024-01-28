using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstBullet : PCStats
{

    Slider slider;
    public Image firstBulletBar;
    public Color greenBullet;
    public Color redBullet;
    public Color blueBullet;
    public Color blackBullet;
    private BulletType type;
    void Start()
    {
        slider = GetComponent<Slider>();
        type = FindObjectOfType<BulletType>();
        slider.value = slider.maxValue;
    }
    void Update()
    {
        slider.maxValue = type.GiveFirstBulletCount();
        slider.value = type.GetFirstbulletCount();
        if(type.GetFirstBullet() == BulletType.bulletType.Red)
        {
            firstBulletBar.color = redBullet;
        }
        if (type.GetFirstBullet() == BulletType.bulletType.Green)
        {
            firstBulletBar.color = greenBullet;
        }
        if (type.GetFirstBullet() == BulletType.bulletType.Blue)
        {
            firstBulletBar.color = blueBullet;
        }
        if (type.GetFirstBullet() == BulletType.bulletType.Default)
        {
            firstBulletBar.color = blackBullet;
        }
    }
}
