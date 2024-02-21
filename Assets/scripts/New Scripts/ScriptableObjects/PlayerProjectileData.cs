using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ProjectileData/PlayerProjectiles")]
public class PlayerProjectileData : ScriptableObject
{
    public int damage;
    public GameObject bulletToSpawn;
    public float timeBetweenBullets;
    public float maxDistance;
    public float maxTime;
}

