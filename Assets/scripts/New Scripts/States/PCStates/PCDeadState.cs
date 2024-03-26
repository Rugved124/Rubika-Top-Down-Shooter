using System;
using System.Collections.Generic;
using UnityEngine;

public class PCDeadState : BaseState
{
    private PC _pc;
    float timeBeforeDying;
    public PCDeadState(PC pc) : base(pc.gameObject)
    {
        _pc = pc;
    }

    public override void EnterState()
    {
        _pc.playerRb.velocity = Vector3.zero;
        timeBeforeDying = 1.9f;
        _pc.isDead = true;
        _pc.anim.SetTrigger("isDead");
    }

    public override Type ExecuteState()
    {
        timeBeforeDying -= Time.deltaTime;
        if(timeBeforeDying <= 0.9f)
        {
            if(_pc.fadeAnimation != null)
            {
                _pc.fadeAnimation.SetTrigger("Died");
            }
        }
        if(timeBeforeDying <= 0)
        {

            //_pc.Die();
        }
        if(timeBeforeDying <= -1f)
        {
            GameManager.Instance.ReloadScene();
        }
        if(_pc.currentHP > 0)
        {
            _pc.isDead = false;
            return typeof(PCDefaultState);
        }
        return null;
    }
}
