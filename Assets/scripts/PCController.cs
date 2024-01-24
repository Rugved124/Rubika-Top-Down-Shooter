using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    Rigidbody playerRb;

    [SerializeField]
    float playerMoveSpeed = 5f;

    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotation();
    }

    void PlayerMove()
    {
        Vector3 movementVector = new Vector3(InputManager.instance.GetMovementHorizontal(), playerRb.velocity.y, InputManager.instance.GetMovementVertical());
        //playerRb.AddForce(movementVector.normalized * playerMoveSpeed / Time.fixedDeltaTime);
        playerRb.velocity = movementVector.normalized * playerMoveSpeed / Time.deltaTime;
    }

    void PlayerRotation()
    {
        
        Vector3 lookDir = InputManager.instance.GetMousePosition();
        lookDir.y = 0;
        Debug.DrawLine(transform.position, lookDir);
        transform.LookAt(lookDir);
    }
}
