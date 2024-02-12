using System;
using System.Collections.Generic;
using UnityEngine;

public class PCConsumeState : BaseState
{
    private PC _pc;

    public PCConsumeState(PC pc) : base(pc.gameObject)
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
        if (_pc.beingConsumed != null)
        {
            _pc.Consume(_pc.beingConsumed);
        }
        else
        {
            return typeof(PCDefaultState);
        }
        return null;
    }
}
