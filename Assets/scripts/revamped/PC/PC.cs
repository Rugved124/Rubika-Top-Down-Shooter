using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Events;
using System.Collections;
public class PC : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState currentState { get; private set; }

    public float pcSpeed;

    public float consumeRange;

    [SerializeField]
    private bool isInvincible;

    public float slowMultiplier = 1;

    public Rigidbody playerRb;

    public Transform bulletSpawn;

    public bool isConsuming;

    public GameObject beingConsumed;

    public GameObject visuals;

    public Animator anim;

    public int maxHP;
    public int currentHP;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Image healthBar;

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

    public LayerMask soul;

    [SerializeField]
    AudioSource consume;
    bool consumeSound;

    [SerializeField]
    AudioSource burp;

    bool isBurnt;

    [SerializeField]
    private GameObject floatingTextPrefab;

    [SerializeField]
    private GameObject healEffect;

    [SerializeField]
    Slider dashCooldownUI;

    [SerializeField]
    Slider shieldHPUI;

    [SerializeField]
    private GameObject lowHealthFeedBack;

    private float blinkSpeed;
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
        if (GameManager.Instance.CanLoadData() && SaveManager.LoadPlayerPosition() != Vector3.zero)
        {
            LoadPlayerData();
            GameManager.Instance.ReduceLoadCount();
        }
        else
        {
            transform.position = GameManager.Instance.respawnPoint;
            currentHP = maxHP;
            SaveManager.SavePlayerStats(transform.position, maxHP, AmmoManager.EquippedAmmoType.DEFAULTAMMO, AmmoManager.EquippedAmmoType.DEFAULTAMMO, 20, 1);
        }
        crosshair = GetComponentInChildren<CrossHairPos>().gameObject;
        respawnPoint = transform.position;
        consumeLine.SetActive(false);
        anim = visuals.GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
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
        isCollided = false;
        isBurnt = false;
        consumeSound = true;
        dashCooldownUI.maxValue = dashCooldown;
        dashCooldownUI.value = dashCooldownUI.maxValue;
        if(healEffect != null)
        {
            healEffect.SetActive(false);
        }
        if(lowHealthFeedBack != null)
        {
            lowHealthFeedBack.SetActive(false);
        }
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

        if (InputManager.instance.GetDashButton() && canDash && !InputManager.instance.GetIfConsumeIsHeld() && !isDead && GameManager.Instance.currentState == GameManager.GameStates.RUNNING)
        {
            float forwards = InputManager.instance.GetMovementVertical();
            float sideways = InputManager.instance.GetMovementHorizontal();
            Vector3 dashVector = new Vector3(sideways, 0, forwards);
            StartCoroutine(Dash(dashVector));
        }
        if (AmmoManager.instance.currentShield != null && shieldHPUI != null)
        {
            shieldHPUI.value = AmmoManager.instance.currentShield.GetComponent<ShieldBehaviour>().GetCurrentHitPoints();
            shieldHPUI.maxValue = AmmoManager.instance.currentShield.GetComponent<ShieldBehaviour>().maxShieldCount;
        }
        if(AmmoManager.instance.currentShield == null && shieldHPUI != null)
        {
            shieldHPUI.maxValue = 1;
            shieldHPUI.value = 0;
        }
        slider.value = currentHP;
        float hpPercent = (float)currentHP / (float)maxHP;
        if(lowHealthFeedBack != null)
        {
            lowHealthFeedBack.SetActive(true);
            if (hpPercent <= 0.3)
            {
                blinkSpeed -= Time.deltaTime;
                if (blinkSpeed <= 0)
                {
                    blinkSpeed = 1f;
                    lowHealthFeedBack.GetComponent<Image>().color = new Color(1, 0, 0, 0.15f);
                }
                else if(blinkSpeed <= 0.5 && blinkSpeed > 0)
                {
                    lowHealthFeedBack.GetComponent<Image>().color = new Color(1, 0, 0, 0.01f);
                }

                
            }
            else
            {
                lowHealthFeedBack.GetComponent<Image>().color = new Color(1, 0, 0, 0f);
            }
        }
        //-------------------------------------------------------------HP Bar Colour Change------------------------------------------------------------------------
        //if (healthBar != null)
        //{
        //    // Calculate inverted values
        //    float invertedHpPercent = 1 - hpPercent;

        //    // Adjusted threshold for color change
        //    float threshold = 0.3f;

        //    // If health percentage is below the threshold, adjust color to red
        //    if (hpPercent <= threshold)
        //    {
        //        healthBar.color = new Vector4(1, 0, 0, 1); // Full red color
        //    }
        //    else
        //    {
        //        // Linear interpolation between green and red
        //        float greenComponent = Mathf.Lerp(0, 1, (hpPercent - threshold) / (1 - threshold));
        //        float redComponent = Mathf.Lerp(1, 0.4f, (hpPercent - threshold) / (1 - threshold));
        //        float blueComponent = Mathf.Lerp(0, 0.7f, (hpPercent - threshold) / (1 - threshold));
        //        // Assign color with adjusted values
        //        healthBar.color = new Vector4(redComponent, greenComponent, blueComponent, 1);
        //    }
        //}
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
            Burning();
        }
    } 
    public void PlayerMove(Vector3 movement, float slowMultiplier)
    {
        Quaternion rotation = Quaternion.AngleAxis(-45, Vector3.up);
        camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
        //Quaternion rotation =  Quaternion.AngleAxis(Vector3.SignedAngle(camForward, transform.forward,Vector3.up) - 45, Vector3.up);
        movement = rotation * movement;
        //anim.SetFloat("Forwards", InputManager.instance.GetMovementVertical());
        //anim.SetFloat("Sideways", InputManager.instance.GetMovementHorizontal()) ;
        //playerRb.MovePosition(transform.position + (movement * pcSpeed * slowMultiplier * Time.deltaTime));
        //playerRb.AddForce((movement * pcSpeed * slowMultiplier) * Time.deltaTime);
        playerRb.velocity = movement * pcSpeed * slowMultiplier ;
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
        if (consumeSound)
        {
            consumeSound = false;
            consume.Play();
        }
        consumeObj.GetComponent<Souls>().Consumption();
    }

    public void DoneConsuming()
    {
        consumeSound = true;
        consume.Pause();
        burp.Play();
        beingConsumed = null;
    }
    public void TakeDamage(int damage)
    {
        if (!isInvincible || isDashing)
        {
            currentHP -= damage;
            if (floatingTextPrefab != null)
            {
                var number = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
                number.GetComponent<FloatingText>().SetDamageNumber(damage);
            }
        }

    }
    public void Heal(int healnumber)
    {
        currentHP += healnumber;
        if(healnumber != 0)
        {
            if (floatingTextPrefab != null)
            {
                var number = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
                number.GetComponent<FloatingText>().SetToHeal(healnumber);
            }
            StartCoroutine(HealVFX());
        }

    }
    public void TakeDamageOverTime(int damage)
    {
        if (!isInvincible)
        {
            currentHP -= damage;
            if (floatingTextPrefab != null)
            {
                var number = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
                number.GetComponent<FloatingText>().SetDamageNumber(damage);
            }
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
        timeBetweenFire -= Time.deltaTime;
        if (timeBetweenFire <= 0f)
        {
            timeBetweenFire = statusEffects.burnTickSpeed;
            TakeDamageOverTime(statusEffects.burningPerTick);

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
            if (other.tag == "SlowPC" && !isDashing)
            {
                statusEffects.isSlowedCounting = true;
                statusEffects.isSlowed = true;
                statusEffects.hasLostAbility = true;
            }
            if (other.tag == "Poison" && !isDashing)
            {
                statusEffects.isPoisonCounting = true;
                statusEffects.isPoisoned = true;
            }
            if(other.tag == "Fire" && !isDashing)
            {
                if(!isBurnt)
                {
                    isBurnt = true;
                    TakeDamage(statusEffects.burstDamage);
                }
                isBurningFor = Time.time;
            }

        }
        if (!other.gameObject.activeInHierarchy || other.gameObject == null)
        {
            StartCoroutine(EResetBurnt());
            statusEffects.isPoisonCounting = false;
            statusEffects.isSlowedCounting = false;
            statusEffects.hasLostAbility = false;
            slowMultiplier = 1;
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
            if(other.tag == "Fire")
            {
                StartCoroutine(EResetBurnt());
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
        //damageIndicator.SetActive(false);
        consumeSound = true;
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

        if (isDashing)
        {
            isDashing = false;
            playerRb.useGravity = true;
            playerRb.velocity = Vector3.zero;
            if(dashCooldownUI != null)
            {
                dashCooldownUI.value = 0f;
            }
        }
        while (dashCooldownUI.value <= dashCooldown)
        {
            if (dashCooldownUI != null)
            {
                dashCooldownUI.value += Time.deltaTime;
            }
            if(dashCooldownUI.value >= dashCooldown)
            {
                break;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(dashCooldown);
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

    IEnumerator IDamageTaken()
    {
        yield return new WaitForSeconds(0.15f);
    }

    IEnumerator EResetBurnt()
    {
        yield return new WaitForSeconds(0.8f);
        isBurnt = false;
    }
    private void LoadPlayerData()
    {
        currentHP = SaveManager.LoadPlayerHealth();
        transform.position = SaveManager.LoadPlayerPosition();

        Debug.Log("Player HP" + currentHP);
    } 

    IEnumerator HealVFX()
    {
        healEffect.SetActive(true);

        yield return new WaitForSeconds(1f);

        healEffect.SetActive(false);
    }
}
