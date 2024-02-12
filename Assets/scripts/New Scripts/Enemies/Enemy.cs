using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using JetBrains.Annotations;

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

    protected FiniteStateMachine fsm;

    public EnemyData enemyData;
    public PC pc { get; private set; }

    protected float firedTime;

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
    private int curShieldPoints;
    public bool isShielded;
    public bool canShield;

    public float hpPercent;
    public bool canRunAway;
    float timer = 4f;
    float randomAngle = 0f;
    private void Awake()
    {
        firedTime = enemyData.timeBetweenBullets;
        fsm = GetComponent<FiniteStateMachine>();
        pc = FindObjectOfType<PC>();
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
        hpPercent = (currentHP / maxHP) * 100f;
        if (curShieldPoints <= 0)
        {
            isShielded = false;
        }
           

        hpSlider.value = currentHP;
        if(currentHP <= 0)
        {
             Die();
        }
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
        DropSoul(enemySoul);
        Destroy(this.gameObject);
    }

    public virtual void  ResetAttack()
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
        if(curShieldPoints > 0)
        {
            curShieldPoints--;
        }
        else if(curShieldPoints <= 0)
        {
            currentHP -= damage;
        }
       
    }
    
    void DropSoul(GameObject enemySoul)
    {
        Instantiate(enemySoul, transform.position, Quaternion.identity);
    }
    public virtual void SecondaryWeaponFire() 
    {

    }
    public virtual void LookForAllies()
    {

    }
    public void AddShield()
    {
        isShielded = true;
        curShieldPoints = 5;
    }
    public virtual void ResetShield()
    {

    }

    public virtual void RunAwayWhenLow()
    {
        Vector3 runDir = (transform.position - pc.transform.position).normalized * enemyData.runAwayDistance;
       
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            randomAngle = Random.Range(-45f, 45f);
            timer = 4f; 
        }
        runDir = Quaternion.AngleAxis(randomAngle,Vector3.up) * (runDir + transform.position);
        agent.SetDestination(runDir);
    }
}

