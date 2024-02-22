using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;

public class PC : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState currentState { get; private set; }

    public float pcSpeed;

    public float consumeRange;


    public float slowMultiplier = 1;

    Rigidbody playerRb;

    public Transform bulletSpawn;

    public bool isConsuming;

    public GameObject beingConsumed;

    public GameObject visuals;
    
    public Animator anim;

    [SerializeField]
    int maxHP;
    public int currentHP;

    [SerializeField]
    private Slider slider;

    public PCStatusEffectsData statusEffects;

    GameObject bullet;
    
    public float isBurningFor;
    [SerializeField]
    private float maxBurnTime;
    public int nannyFire;
    public float timeBetweenFire;

    public bool isDead;
    public void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(PCDefaultState), new PCDefaultState(this)},
            { typeof(PCConsumeState), new PCConsumeState(this)},
            { typeof(PCDeadState), new PCDeadState(this)}
        };
        SetStates(states);


    }
    private void Awake()
    {
        InitializeStateMachine();
    }

    private void Start()
    {
        anim = visuals.GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        currentHP = maxHP;
        slider.maxValue = maxHP;
        statusEffects.burnLastTick = 0f;
        statusEffects.burnNumber = 0;
        statusEffects.lastTick = 0f;
        statusEffects.isPoisonCounting = false;
        statusEffects.isPoisoned = false;
        statusEffects.isSlowed = false;
        statusEffects.isSlowedCounting = false;
        statusEffects.hasLostAbility = false;
        timeBetweenFire = statusEffects.burnTickSpeed;
        isBurningFor = -maxBurnTime;
    }

    private void Update()
    {
        anim.SetBool("isConsuming", isConsuming);
        if (currentState == null)
        {
            currentState = availableStates.Values.First();
        }
        else
        {
            UpdateState();
        }

        slider.value = currentHP;
        //-------------------------------------StatusEfffects---------------------------------------------------------
        if (statusEffects.isPoisoned)
        {
            if (statusEffects.isPoisonCounting)
            {
                statusEffects.poisonedForTime = Time.time;
            }
            Poisoned();
        }
        if (statusEffects.isSlowed)
        {
            if (statusEffects.isSlowedCounting)
            {
                statusEffects.slowedForTime = Time.time;
            }
            Slowed();
        }
        if (Time.time - isBurningFor <= maxBurnTime)
        {
            nannyFire = 1;
        }
        else
        {
            nannyFire = 0;
        }

        Burning();
        NannyBurning();
    }
    public void PlayerMove(Vector3 movement, float slowMultiplier)
    {
        Quaternion rotation = Quaternion.AngleAxis(-45, Vector3.up);
        movement = rotation * movement;
        playerRb.MovePosition(transform.position + (movement * pcSpeed * slowMultiplier * Time.deltaTime));
    }

    public void PlayerRotation()
    {
        Vector3 lookDir = InputManager.instance.GetMousePosition();
        lookDir.y = transform.position.y;
        transform.LookAt(lookDir);
    }

    public Transform GetPCShoot()
    {
        return bulletSpawn;
    }

    public void Consume(GameObject consumeObj)
    {
        consumeObj.GetComponent<Souls>().Consumption();
    }
    
    public void DoneConsuming()
    {
       
        beingConsumed = null;
    }
    public void TakeDamage(int damage)
    {
      //  currentHP -= damage;
    }

    public void Die()
    {
       this.gameObject.SetActive(false);
    }
    public void KnockBack(Vector3 collisionPos, float pushBackForce)
    {
        Debug.Log("Pushed");
        Vector3 knockBackDir = transform.position - collisionPos;
        playerRb.AddForce(knockBackDir.normalized * pushBackForce,ForceMode.Impulse);
    }
    //---------------------------------------------------PC Status Effect Functions Start--------------------------------------------------------
    void Poisoned()
    {
        if (Time.time - statusEffects.poisonedForTime <= statusEffects.maxPoisonedForTime)
        {

            if (Time.time - statusEffects.lastTick >= statusEffects.tickSpeed)
            {
                slowMultiplier = statusEffects.slowedSpeed;
                TakeDamage(statusEffects.poisonDamagePerTick);
                statusEffects.lastTick = Time.time;
            }

        }
        else
        {
            statusEffects.isPoisoned = false;
            slowMultiplier = 1f;
        }
    }
    void Burning()
    {
        if (Time.time - statusEffects.burnLastTick >= statusEffects.burnTickSpeed)
        {
            TakeDamage(statusEffects.burningPerTick * statusEffects.burnNumber);
            statusEffects.burnLastTick = Time.time;
        }
    }
    void NannyBurning()
    {
        timeBetweenFire -= Time.deltaTime;
        if (timeBetweenFire <= 0f)
        {
            timeBetweenFire = statusEffects.burnTickSpeed;
            TakeDamage(statusEffects.burningPerTick * nannyFire);

        }
    }
    void Slowed()
    {
        if (Time.time - statusEffects.slowedForTime <= statusEffects.maxSlowedForTime)
        {
            slowMultiplier = statusEffects.slowedSpeed;
        }
        else
        {
            statusEffects.isSlowed = false;
            slowMultiplier = 1;
            return;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                statusEffects.isSlowedCounting = true;
                statusEffects.isSlowed = true;
                statusEffects.hasLostAbility = true;

            }
            if (other.tag == "Poison")
            {
                statusEffects.isPoisonCounting = true;
                statusEffects.isPoisoned = true;
            }
            if(other.tag == "NannyFire")
            {
                isBurningFor = Time.time;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                statusEffects.isSlowedCounting = false;
                statusEffects.hasLostAbility = false;
                slowMultiplier = 1;
            }
            if (other.tag == "Poison")
            {
                statusEffects.isPoisonCounting = false;
            }
        }
    }
    //----------------------------------------------------------------Collisions and Triggers for PC End---------------------------------------------------------

    public void SetStates(Dictionary<Type, BaseState> states)
    {
        availableStates = states;
    }
    void SwitchToNextState(Type nextState)
    {
        currentState = availableStates[nextState];
        currentState.EnterState();
    }

    void UpdateState()
    {
        Type nextState = currentState.ExecuteState();

        if (nextState != null && nextState != currentState.GetType())
        {
            SwitchToNextState(nextState);
        }
    }
}
