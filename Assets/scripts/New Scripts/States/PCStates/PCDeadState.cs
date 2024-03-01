using System;
using System.Collections.Generic;
using UnityEngine;

public class PCDeadState : BaseState
{
    private PC _pc;

    public PCDeadState(PC pc) : base(pc.gameObject)
    {
        _pc = pc;
    }

    public override void EnterState()
    {
        _pc.isDead = true;
        _pc.Die();
    }

    public override Type ExecuteState()
    {
        if(_pc.currentHP > 0)
        {
            _pc.isDead = false;
            return typeof(PCDefaultState);
        }
        return null;
    }
}
