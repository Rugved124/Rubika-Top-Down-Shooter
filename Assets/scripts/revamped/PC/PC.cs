using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;

public class PC : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState currentState { get; private set; }

    public float pcSpeed;

    public float consumeRange;

    [SerializeField]
    private bool isInvincible;

    public float slowMultiplier = 1;

    Rigidbody playerRb;

    public Transform bulletSpawn;

    public bool isConsuming;

    public GameObject beingConsumed;

    public GameObject visuals;

    public Animator anim;

    public int maxHP;
    public int currentHP;

    [SerializeField]
    private Slider slider;

    public PCStatusEffectsData statusEffects;

    public GameObject consumeLine;

    public float isBurningFor;
    [SerializeField]
    private float maxBurnTime;
    public int nannyFire;
    public float timeBetweenFire;

    public bool isDead;
    public Vector3 respawnPoint;

    public Transform cam;
    Vector3 camForward;
    public Animator fadeAnimation;
    //------------------------------------Dash Vars
    [SerializeField]
    float dashRange;
    [SerializeField]
    float dashSpeed;
    public bool canDash;
    public bool isDashing;
    [SerializeField]
    float dashCooldown;
    public GameObject crosshair;
    bool isCollided;
    public void InitializeStateMachine()
    {
        Dictionary<Type, BaseState> states = new Dictionary<Type, BaseState>()
        {
            { typeof(PCDefaultState), new PCDefaultState(this)},
            { typeof(PCConsumeState), new PCConsumeState(this)},
            { typeof(PCDeadState), new PCDeadState(this)},
            { typeof(PCDashState), new PCDashState(this)}
        };
        SetStates(states);


    }
    private void Awake()
    {
        InitializeStateMachine();
    }

    private void Start()
    {
        transform.position = GameManager.Instance.respawnPoint;
        crosshair = GetComponentInChildren<CrossHairPos>().gameObject;
        respawnPoint = transform.position;
        consumeLine.SetActive(false);
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
        cam = Camera.main.transform;
        canDash = true;
        dashCooldown = dashRange / dashSpeed + dashCooldown;
        isCollided = false;
    }

    private void Update()
    {
        if (currentState == null)
        {
            currentState = availableStates.Values.First();
        }
        else
        {
            UpdateState();
        }
        //-------------------------------------Dash Things-------------------------------------

        if (InputManager.instance.GetDashButton() && canDash && !InputManager.instance.GetIfConsumeIsHeld())
        {
            float forwards = InputManager.instance.GetMovementVertical();
            float sideways = InputManager.instance.GetMovementHorizontal();
            Vector3 dashVector = new Vector3(sideways, 0, forwards);
            StartCoroutine(Dash(dashVector));
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
        camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
        //Quaternion rotation =  Quaternion.AngleAxis(Vector3.SignedAngle(camForward, transform.forward,Vector3.up) - 45, Vector3.up);
        movement = rotation * movement;
        //anim.SetFloat("Forwards", InputManager.instance.GetMovementVertical());
        //anim.SetFloat("Sideways", InputManager.instance.GetMovementHorizontal()) ;
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
        if (!isInvincible || isDashing)
        {
            currentHP -= damage;
        }

    }
    public void TakeDamageOverTime(int damage)
    {
        if (!isInvincible)
        {
            currentHP -= damage;
        }
    }
    //-----------------------------------------------Diying and Respawning---------------------------------------------------------------
    public void Die()
    {
        if(AmmoManager.instance.currentShield != null) 
        {
            Destroy(AmmoManager.instance.currentShield.gameObject);
        }
        this.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        currentHP = maxHP;
        transform.position = respawnPoint;
    }
    //-------------------------------------------------------------------------------------------------------------------
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
                TakeDamageOverTime(statusEffects.poisonDamagePerTick);
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
            TakeDamageOverTime(statusEffects.burningPerTick * nannyFire);

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
    public void ResetPCStats()
    {
        AmmoManager.instance.ResetEquippedAmmo();
        AmmoManager.instance.RemoveCurrentShield();
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
        isInvincible = false;
        canDash = true;
        slowMultiplier = 1f;
        isCollided = false;
    }
    public IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        playerRb.useGravity = false;
        Quaternion rotation = Quaternion.AngleAxis(-45, Vector3.up);
        direction = rotation * direction;
        playerRb.velocity = direction.normalized * dashSpeed;

        float dashDuration = dashRange / dashSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < dashDuration && !isCollided) // Check if collided during dash
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        playerRb.useGravity = true;
        playerRb.velocity = Vector3.zero;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        isCollided = false;

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            if (collision != null)
            {
                if (!collision.collider.CompareTag("Ground"))
                {
                    isCollided = true;
                }
            }

        }
    }
}
