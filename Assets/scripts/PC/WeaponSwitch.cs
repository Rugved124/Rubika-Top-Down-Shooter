using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;
    public BulletType.bulletType bulletTypeForShooting;
    void Start()
    {
        
        SwitchWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTypeForShooting = BulletType.bulletState.GetBulletTypeForShooting();
        int previousSelectedWeapon = selectedWeapon;
        if (bulletTypeForShooting == BulletType.bulletType.Default)
        {
            selectedWeapon = 0;
        }
        if (bulletTypeForShooting == BulletType.bulletType.Red)
        {
            selectedWeapon = 1;
        }
        if (bulletTypeForShooting == BulletType.bulletType.Blue)
        {
            selectedWeapon = 2;
        }
        if (bulletTypeForShooting == BulletType.bulletType.Green)
        {
            selectedWeapon = 3;
        }
        if (bulletTypeForShooting == BulletType.bulletType.RedGreen)
        {
            selectedWeapon = 4;
        }
        if (bulletTypeForShooting == BulletType.bulletType.RedBlue)
        {
            selectedWeapon = 5;
        }
        if (bulletTypeForShooting == BulletType.bulletType.BlueGreen)
        {
            selectedWeapon = 6;
        }
        if (previousSelectedWeapon != selectedWeapon) 
        {
            SwitchWeapon();
        }
        //Debug.Log(selectedWeapon);
    }
    void SwitchWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
            //Debug.Log(i);
        }
    }
}
