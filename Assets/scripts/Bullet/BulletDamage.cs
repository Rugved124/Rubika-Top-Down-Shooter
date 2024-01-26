using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage;
    void Start()
    {
        damage = 20;
    }

    public int GetBulletDamage()
    {
        return damage;
    }
}
