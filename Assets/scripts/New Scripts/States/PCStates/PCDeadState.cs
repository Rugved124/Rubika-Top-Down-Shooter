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
        _pc.Die();
    }

    public override Type ExecuteState()
    {
        
        return null;
    }
}
