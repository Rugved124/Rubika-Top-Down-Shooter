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
    float slowMultiplier = 1;

    Rigidbody playerRb;
    [SerializeField]
    private Transform bulletSpawn;

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
        Vector3 movetVector = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;

        //Check whether Poisoned or Slowed
        if (currentPCState == PCStates.SLOWED || currentPCState == PCStates.POISONED)
        {
            slowMultiplier = 0.5f;
        }
        else
        {
            slowMultiplier = 1f;
        }
        PlayerMove(movetVector, slowMultiplier);

        //Check whether PC is Consuming
        if (currentPCState != PCStates.CONSUME)
        {
            PlayerMove(movetVector, slowMultiplier);
            PlayerRotation();
        }
        //-------------------------------------Movement Section End-----------------------------------------------------------------------------------
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
        playerRb.MovePosition(transform.position + (movement * pcSpeed * slowMultiplier * Time.deltaTime));
    }

    private void PlayerRotation()
    {
        Vector3 lookDir = InputManager.instance.GetMousePosition();
        lookDir.y = transform.position.y;
        Debug.DrawLine(transform.position, lookDir);
        transform.LookAt(lookDir);
    }

    public Vector3 GetPCShootPos()
    {
        return bulletSpawn.position;
    }
}
