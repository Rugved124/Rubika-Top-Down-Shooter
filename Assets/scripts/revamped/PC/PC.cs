using UnityEditor.Rendering.LookDev;
using UnityEngine;

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

    private void Start()
    {
        currentPCState = PCStates.DEFAULT;
        playerRb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        //Check if PC is dead
        if (currentPCState == PCStates.DEAD)
        {
            return;
        }
        //-------------------------------------Movement Section-----------------------------------------------------------------------------------
        Vector3 moveVector = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;

        //Check whether Poisoned or Slowed
        if (currentPCState == PCStates.SLOWED || currentPCState == PCStates.POISONED)
        {
            slowMultiplier = 0.5f;
        }
        else
        {
            slowMultiplier = 1f;
        }
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
            case PCStates.SHIELDED:
                break;
            case PCStates.POISONED:
                break;
            case PCStates.SILENCED:
                break;
            case PCStates.SLOWED:
                break;
            case PCStates.DEAD:
                break;
            case PCStates.BURNED:
                break;
        }
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
}
