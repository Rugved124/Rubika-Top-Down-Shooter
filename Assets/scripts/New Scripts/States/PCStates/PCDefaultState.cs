using System;
using System.Collections.Generic;
using UnityEngine;
using static PC;

public class PCDefaultState : BaseState
{
    private PC _pc;

    public PCDefaultState(PC pc) : base(pc.gameObject)
    {
        _pc = pc;
    }

    public override void EnterState()
    {

    }

    public override Type ExecuteState()
    {
        if (_pc.currentHP <= 0)
        {
            return typeof(PCDeadState);
        }
        Vector3 moveVector = new Vector3(InputManager.instance.GetMovementHorizontal(), 0, InputManager.instance.GetMovementVertical()).normalized;

        _pc.PlayerMove(moveVector, _pc.slowMultiplier);
        _pc.PlayerRotation();

        if (InputManager.instance.GetIfConsumeIsHeld())
        {
            RaycastHit hitObj;
            bool didHit = Physics.Raycast(transform.position, transform.forward, out hitObj, _pc.consumeRange);
            Debug.DrawRay(transform.position, transform.forward * _pc.consumeRange, Color.red);
            if (didHit)
            {
                if (hitObj.collider.CompareTag("Consumables"))
                {
                    if (_pc.beingConsumed == null)
                    {
                        _pc.beingConsumed = hitObj.collider.gameObject;
                    }
                    return typeof(PCConsumeState);

                }
            }
        }
        return null;
    }
}
