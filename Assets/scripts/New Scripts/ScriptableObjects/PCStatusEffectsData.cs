using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "StatusEffectsData/PCData")]
public class PCStatusEffectsData : ScriptableObject
{
    public bool isSlowed, isPoisoned, hasLostAbility;
    
    public float normalSpeed;
    
    public float maxSlowedForTime, maxPoisonedForTime;

    public float slowedSpeed = 0.3f;

    public float poisonedForTime, slowedForTime;

    public int poisonDamagePerTick, burningPerTick;

    [HideInInspector]
    public int burnNumber;

    public float tickSpeed, burnTickSpeed;

    public float lastTick, burnLastTick;


    public bool isSlowedCounting, isPoisonCounting;
}
