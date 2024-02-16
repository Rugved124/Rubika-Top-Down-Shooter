using System;
using System.Collections.Generic;
using UnityEngine;

public class NannyTiredState : BaseState
{
    Enemy _enemy;
    float coolDown;
    public NannyTiredState(Enemy enemy): base(enemy.gameObject)
    {
        _enemy = enemy;
    }

    public override void EnterState()
    {
        coolDown = 3f;
    }

    public override Type ExecuteState()
    {
        coolDown -=Time.deltaTime;
        if(coolDown <= 0f)
        {
            return typeof(NannyIdleState);
        }
        return null;
    }
}
