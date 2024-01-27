using UnityEngine;
using UnityEngine.Rendering;

public class PCStatusEffects : PCStats
{
    public static PCStatusEffects instance;

    public bool isSlowed, isPoisoned, hasLostAbility, consumeIsDisabled;

    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float maxSlowedForTime, maxPoisonedForTime, maxSilencedForTime;
   
    [SerializeField]
    private float slowedSpeed = 2;

    [SerializeField]
    private float poisonedForTime, slowedForTime, silencedForTime;

    public int poisonDamagePerTick;

    [SerializeField]
    private float tickSpeed, lastTick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        normalSpeed = PCController.instance.playerMoveSpeed;
    }
    void Update()
    {
        if (isPoisoned)
        {
            if (PCCollisionHandler.instance.isPoisonCounting)
            {
                poisonedForTime = Time.time;
            }
            Poisoned();
        }
        if (isSlowed)
        {
            if (PCCollisionHandler.instance.isSlowedCounting)
            {
                slowedForTime = Time.time;
            }
            Slowed();
        }
  
        if (hasLostAbility)
        {
            if (PCCollisionHandler.instance.isSilenceCounting)
            {
                silencedForTime = Time.time;
            }
            LostAbility();
        }
            
    }
    void Poisoned()
    {
        if (Time.time - poisonedForTime <= maxPoisonedForTime)
        {
            
            if (Time.time - lastTick >= tickSpeed)
            {
                
                PCHealth.instance.TakeDamage(poisonDamagePerTick);
                lastTick = Time.time;
            }
            
        }
        else isPoisoned = false;
    }

    void Slowed()
    {
        if (Time.time - slowedForTime <= maxSlowedForTime)
        {
            PCController.instance.playerMoveSpeed = slowedSpeed;   
        }
        else
        {
            isSlowed = false;
            PCController.instance.playerMoveSpeed = normalSpeed;
            return;
        }

    }

    void LostAbility()
    {
        if (Time.time - silencedForTime <= maxSilencedForTime)
        {
            consumeIsDisabled = true;
        }
        else
        {
            hasLostAbility = false;
            consumeIsDisabled = false;
        }
    }
}
