using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PC;

public class PCDefaultState : BaseState
{
    float forwards;
    float sideways;
    private PC _pc;
    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardsAmount;
    float sidewaysAmount;
    public PCDefaultState(PC pc) : base(pc.gameObject)
    {
        _pc = pc;
        cam = Camera.main.transform;
    }

    public override void EnterState()
    {
    }

    public override Type ExecuteState()
    {
        if(GameManager.Instance.currentState == GameManager.GameStates.RUNNING)
        {
            forwards = InputManager.instance.GetMovementVertical();
            sideways = InputManager.instance.GetMovementHorizontal();
            if (_pc.currentHP <= 0)
            {
                return typeof(PCDeadState);
            }
            if (_pc.isDashing)
            {
                return typeof(PCDashState);
            }
            Vector3 moveVector = new Vector3(sideways, 0, forwards).normalized;
            if (cam != null)
            {
                camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
                move = forwards * camForward + sideways * cam.right;
            }
            else
            {
                move = forwards * Vector3.forward + sideways * Vector3.right;
            }

            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            Move(move);
            _pc.PlayerMove(moveVector, _pc.slowMultiplier);
            _pc.PlayerRotation();

            if (InputManager.instance.GetIfConsumeIsHeld() && !_pc.isDashing)
            {
                _pc.consumeLine.SetActive(true);
                RaycastHit hitObj;
                bool didHit = Physics.Raycast(_pc.bulletSpawn.position, transform.forward, out hitObj, _pc.consumeRange, _pc.soul);
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
            else
            {
                _pc.consumeLine.SetActive(false);
            }
        }
        return null;
    }

    void Move(Vector3 move)
    {
        if(move.magnitude > 1)
        {
            move.Normalize();
        }
        this.moveInput = move;
        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        sidewaysAmount = localMove.x;
        forwardsAmount = localMove.z;
    }
    void UpdateAnimator()
    {
        _pc.anim.SetFloat("Forwards", forwardsAmount, 0.1f ,Time.deltaTime);
        _pc.anim.SetFloat("Sideways", sidewaysAmount, 0.1f, Time.deltaTime);
    }
}
