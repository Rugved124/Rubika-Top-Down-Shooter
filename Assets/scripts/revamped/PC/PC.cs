using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.UI;
public class PC : MonoBehaviour
{
    public enum PCStates
    {
        DEFAULT,
        CONSUME,
        SHIELDED,
        SLOWED,
        POISONED,
        BURNED,
        SILENCED,
        DEAD
    }

    public PCStates currentPCState;

    [SerializeField]
    private float pcSpeed;

    [SerializeField]
    private float consumeRange;
    [SerializeField]
    float slowMultiplier = 1;

    Rigidbody playerRb;
    [SerializeField]
    private Transform bulletSpawn;

    bool isConsuming;
    GameObject beingConsumed;

    [SerializeField]
    int maxHP;
    [SerializeField]
    private int currentHP;

    [SerializeField]
    private Slider slider;

    //-----------------------------------------StatusEffects For PC-----------------------------------------------------------------
    public bool isSlowed, isPoisoned, hasLostAbility, isBurning;

    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float maxSlowedForTime, maxPoisonedForTime;

    [SerializeField]
    private float slowedSpeed = 0.3f;

    [SerializeField]
    private float poisonedForTime, slowedForTime;

    public int poisonDamagePerTick, burningPerTick;

    [SerializeField]
    private float tickSpeed, lastTick, burnTickSpeed, burnLastTick;

    private BulletType bullet;

    public bool isSlowedCounting, isPoisonCounting;
    //-----------------------------------------StatusEffects For PC End-----------------------------------------------------------
    private void Start()
    {
        currentPCState = PCStates.DEFAULT;
        playerRb = GetComponent<Rigidbody>();

        currentHP = maxHP;
        slider.maxValue = maxHP;

    }

    private void Update()
    {
        slider.value = currentHP;
        if (currentHP <= 0)
        {
            Die(); return;
        }
        //Check if PC is dead
        if (currentPCState == PCStates.DEAD)
        {
            return;
        }
        //-------------------------------------Movement Section-----------------------------------------------------------------------------------
        Vector3 moveVector = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;

        //Check whether PC is Consuming
        if (currentPCState != PCStates.CONSUME)
        {
            PlayerMove(moveVector, slowMultiplier);
            PlayerRotation();
        }
        //-------------------------------------Movement Section End-----------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------
        if (InputManager.instance.GetIfConsumeIsHeld()) 
        {
            RaycastHit hitObj;
            bool didHit = Physics.Raycast(transform.position, transform.forward, out hitObj, consumeRange);
            Debug.DrawRay(transform.position, transform.forward * consumeRange, Color.red);
            if (didHit)
            {
                if (hitObj.collider.CompareTag("Consumables"))
                {
                    ChangePCState(PCStates.CONSUME);
                    if(beingConsumed == null)
                    {
                        beingConsumed = hitObj.collider.gameObject;
                    }
                }
            }
            
        }
        switch(currentPCState)
        {
            case PCStates.DEFAULT:
                break;
            case PCStates.CONSUME:
                if (beingConsumed != null)
                {
                    Consume(beingConsumed); 
                }
                break;
        }
        if (isPoisoned)
        {
            if (isPoisonCounting)
            {
                poisonedForTime = Time.time;
            }
            Poisoned();
        }
        if (isSlowed)
        {
            if (isSlowedCounting)
            {
                slowedForTime = Time.time;
            }
            Slowed();
        }
        
        if (isBurning)
        {
            Burning();
        }
        //if (hasLostAbility)
        //{
        //    LostAbility();
        //}
    }
    void ChangePCState(PCStates state)
    {
        if (currentPCState != state)
        {
            currentPCState = state;
        }
    }

    private void PlayerMove(Vector3 movement, float slowMultiplier)
    {
        Quaternion rotation = Quaternion.AngleAxis(-45, Vector3.up);
        movement = rotation * movement;
        playerRb.MovePosition(transform.position + (movement * pcSpeed * slowMultiplier * Time.deltaTime));
    }

    private void PlayerRotation()
    {
        Vector3 lookDir = InputManager.instance.GetMousePosition();
        lookDir.y = transform.position.y;
        transform.LookAt(lookDir);
    }

    public Transform GetPCShoot()
    {
        return bulletSpawn;
    }

    private void Consume(GameObject consumeObj)
    {
        consumeObj.GetComponent<Souls>().Consumption();
    }
    
    public void DoneConsuming()
    {
        ChangePCState(PCStates.DEFAULT);
        beingConsumed = null;
    }

    public PCStates GetPCState()
    {
        return currentPCState;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }

    private void Die()
    {
       this.gameObject.SetActive(false);
    }
    //---------------------------------------------------PC Status Effect Functions Start--------------------------------------------------------
    void Poisoned()
    {
        if (Time.time - poisonedForTime <= maxPoisonedForTime)
        {

            if (Time.time - lastTick >= tickSpeed)
            {
                slowMultiplier = slowedSpeed;
                TakeDamage(poisonDamagePerTick);
                lastTick = Time.time;
            }

        }
        else
        {
            isPoisoned = false;
            slowMultiplier = 1f;
        }
    }
    void Burning()
    {
        if (Time.time - burnLastTick >= burnTickSpeed)
        {
            TakeDamage(burningPerTick);
            burnLastTick = Time.time;
        }
    }
    void Slowed()
    {
        if (Time.time - slowedForTime <= maxSlowedForTime)
        {
           slowMultiplier = slowedSpeed;
        }
        else
        {
            isSlowed = false;
            slowMultiplier = 1;
            return;
        }

    }

    void LostAbility()
    {
        bullet.LostConsumedSoul();
    }

    //------------------------------------------------------------PC Status Effects Functions End------------------------------------------------------
    //------------------------------------------------------------Collisions and Triggers for PC-------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                isSlowedCounting = true;
                isSlowed = true;
                hasLostAbility = true;

            }
            if (other.tag == "Poison")
            {
                isPoisonCounting = true;
                isPoisoned = true;

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            if (other.tag == "SlowPC")
            {
                isSlowedCounting = false;
                hasLostAbility = false;

            }
            if (other.tag == "Poison")
            {
                isPoisonCounting = false;
            }
        }
    }
    //----------------------------------------------------------------Collisions and Triggers for PC End---------------------------------------------------------
}
