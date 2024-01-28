using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Slider slider;
    public Image secondBulletBar;
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
        slider.maxValue = type.GiveSecondBulletCount();
        slider.value = type.GetSecondbulletCount();
        if (type.GetSecondBullet() == BulletType.bulletType.Red)
        {
            secondBulletBar.color = redBullet;
        }
        if (type.GetSecondBullet() == BulletType.bulletType.Green)
        {
            secondBulletBar.color = greenBullet;
        }
        if (type.GetSecondBullet() == BulletType.bulletType.Blue)
        {
            secondBulletBar.color = blueBullet;
        }
        if (type.GetSecondBullet() == BulletType.bulletType.Default)
        {
            secondBulletBar.color = blackBullet; 
        }
    }
}
