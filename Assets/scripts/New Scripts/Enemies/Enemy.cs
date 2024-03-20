using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class Enemy : MonoBehaviour
{
     
    public enum EnemyType
    {
        DRUNKENSEPOY,
        BUTCHER,
        NANNY,
        SHADOW
    }

    public EnemyType enemyType;

    public Animator enemyAnim;

    protected FiniteStateMachine fsm;

    public EnemyData enemyData;
    public PC pc { get; private set; }

    public Rigidbody rb { get; private set; }
    public float firedTime;

    [SerializeField]
    private GameObject enemySoul;
    public NavMeshAgent agent { get; private set; }

    [HideInInspector]
    public bool isWeaponFiringDone;

    [HideInInspector]
    public float angularSpeedMulitplier;

    [HideInInspector]
    public float maxHP;
    public float currentHP;

    [SerializeField]
    private Slider hpSlider;

    public List<Enemy> lowHpEnemy;

    public float shieldDistance;

    [HideInInspector]
    public bool isShielded;

    [HideInInspector]
    public bool canShield;

    [SerializeField]
    private GameObject shieldToSpawn;

    public GameObject currentShield;

    [HideInInspector]
    public float hpPercent;

    public bool canRunAway;

    [HideInInspector]
    public bool canDash;
    [HideInInspector]
    public bool isCharging;

    [HideInInspector]
    public bool canDashAgain;

    [HideInInspector]
    public Vector3 dashPos;

    float timer = 4f;
    float randomAngle = 0f;
    [HideInInspector]
    public bool tpDone;

    [SerializeField]
    public Vector3 surroundPos;

    public float fireTime;

    [HideInInspector]
    public bool sepoyLookAtPlayer;

    [HideInInspector]
    public bool canGabbarCharge;

    public GameObject storedAOE;
    public LayerMask all;
    //-----------------------------------------------StatusEffects-------------------------------------------------------//
    float burnLastTick;
    float burnTickSpeed;
    int burnDamage;
    float currentTickTime;
    float maxDuration;
    //--------------------
    float poisonedForTime;
    [SerializeField]
    private float maxPoisonedForTime;

    [SerializeField]
    float tickSpeed;
    float lastTick;

    [SerializeField]
    int poisonDamagePerTick;

    public bool isInGravity;

    private GameObject gravityWell;

    [SerializeField]
    private GameObject floatingTextPrefab;

    [HideInInspector]
    public bool canIdle;
    private void Awake()
    {
        poisonedForTime = -maxPoisonedForTime;
        firedTime = enemyData.timeBetweenBullets;
        fsm = GetComponent<FiniteStateMachine>();
        enemyAnim = GetComponent<Animator>();
        pc = FindObjectOfType<PC>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //NavMesh Stuff
        agent.speed = enemyData.enemyMoveSpeed;
        agent.angularSpeed = enemyData.enemyRotationSpeed;

        angularSpeedMulitplier = enemyData.enemyAttackingAngularSpeed;
        maxHP = enemyData.HitPoints;
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
        isWeaponFiringDone = true;
        canRunAway = true;
        InitializeStateMachine();
    }

    public virtual void Start()
    {

    }

    public virtual void InitializeStateMachine()
    {

    }

    public virtual void FireWeapon()
    {

    }

    public virtual void Update()
    {
        if(gravityWell == null)
        {
            isInGravity = false;
        }
        if (currentShield)
        {
            isShielded = false;
        }
        if(currentShield != null)
        {
            isShielded = true;
            currentShield.GetComponent<ShieldBehaviour>().FollowSpawner(transform);
        }
        hpPercent = (currentHP / maxHP) * 100f;

        hpSlider.value = currentHP;
        if (currentHP <= 0)
        {
            Die();
        }
        Burning();
        Poisoned();
    }

    public string GetCurrentState()
    {
        if (fsm != null)
        {
            if (fsm.currentState != null)
            {
                return fsm.currentState.ToString();
            }
            else
            {
                return "No State Detected";
            }
        }

        return "Not Playing";

    }

    public virtual void Die()
    {
        if(currentShield != null)
        {
            currentShield.GetComponent<ShieldBehaviour>().Die();
            currentShield = null;
        }
        AIManager.instance.RemoveFromList(this);
        DropSoul(enemySoul);
        Destroy(this.gameObject);
    }

    public virtual void ResetAttack()
    {
    }
    public virtual void SetFiringToTrue()
    {
        isWeaponFiringDone = false;
    }
    public virtual void LookAtPlayer()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(floatingTextPrefab != null)
        {
            var number = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
            number.GetComponent<FloatingText>().SetDamageNumber(damage);
        }
    }

    void DropSoul(GameObject enemySoul)
    {
        Vector3 spawnPos = transform.position;
        Instantiate(enemySoul, spawnPos, Quaternion.identity);
    }
    public virtual void SecondaryWeaponFire()
    {

    }
    public virtual void LookForAllies()
    {

    }
    public void AddShield()
    {
        currentShield = Instantiate(shieldToSpawn, transform.position, Quaternion.identity);
        currentShield.GetComponent<ShieldBehaviour>().IsPCShield(false);
        currentShield.GetComponent<ShieldBehaviour>().SetParentToEnemy(this.gameObject);
    }
    public virtual void ResetShield()
    {

    }

    public virtual void RunAwayWhenLow()
    {
        Vector3 runDir = (transform.position - pc.transform.position).normalized * enemyData.runAwayDistance;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            randomAngle = Random.Range(-45f, 45f);
            timer = 4f;
        }
        runDir = Quaternion.AngleAxis(randomAngle, Vector3.up) * (runDir + transform.position);
        agent.SetDestination(runDir);
    }

    public void SetRunAwayToTrue()
    {
        Debug.Log("RunAwayCooldDown");
        Invoke("InvokeRunAway", enemyData.runAwayCooldown);
    }

    void InvokeRunAway()
    {
        canRunAway = true;
    }
    public virtual void Teleport()
    {

    }
    public virtual void Dash()
    {

    }

    public virtual void ResetDash()
    {

    }

    public virtual void ReleaseNannyFire()
    {

    }

    public virtual void SetBurning(int damage, float maxTime, float tickSpeed)
    {
        maxDuration = maxTime;
        burnDamage = damage;
        burnTickSpeed = tickSpeed;
    }
    void Burning()
    {
        if(maxDuration > 0f)
        {
            maxDuration -= Time.deltaTime;
            currentTickTime = Time.time;
            if (currentTickTime - burnLastTick > burnTickSpeed)
            {
                burnLastTick = currentTickTime;
                TakeDamage(burnDamage);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Poison"))
        {
            poisonedForTime = Time.time;
        }
        if (other.CompareTag("GravityWell"))
        {
            gravityWell = other.gameObject;
            isInGravity = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GravityWell"))
        {
            isInGravity = false;
        }
    }
    void Poisoned()
    {
        if (Time.time - poisonedForTime <= maxPoisonedForTime)
        {

            if (Time.time - lastTick >= tickSpeed)
            {
                TakeDamage( poisonDamagePerTick);
                lastTick = Time.time;
            }

        }
    }
    public void SetPoisonedForTime()
    {
        poisonedForTime = Time.time;
    }

    public virtual void SetSpecialSound()
    {

    }
}

