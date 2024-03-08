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
        _enemy.enemyAnim.SetTrigger("TiredState");
        coolDown = 3f;
    }

    public override Type ExecuteState()
    {
        if (_enemy.isInGravity)
        {
            return typeof(SuckedState);
        }

        coolDown -=Time.deltaTime;
        if(coolDown <= 0f)
        {
            return typeof(NannyIdleState);
        }
        return null;
    }
}
