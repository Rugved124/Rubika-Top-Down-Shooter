using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PCDashState : BaseState
{
    private PC _pc;
    public PCDashState(PC pc) : base(pc.gameObject)
    {
        _pc = pc;
    }

    public override void EnterState()
    {
    }

    public override Type ExecuteState()
    {
        Debug.Log("Dashing");
        if (!_pc.isDashing)
        {
            return typeof(PCDefaultState);
        }
        return null;
    }
}

