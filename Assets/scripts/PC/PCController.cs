using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : PCStats
{
    public static PCController instance;    



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
    void Update()
    {
        var targetVector = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;
        MoveTowardsTarget(targetVector);

        PlayerRotation();
    }

    void PlayerRotation()
    {   
        Vector3 lookDir = InputManager.instance.GetMousePosition();
        lookDir.y = transform.position.y;
        Debug.DrawLine(transform.position, lookDir);
        transform.LookAt(lookDir);
    }

    private void MoveTowardsTarget(Vector3 targetVector)
    {
        var speed = playerMoveSpeed * Time.deltaTime;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
    }
}
