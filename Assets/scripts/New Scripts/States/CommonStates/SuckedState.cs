using System;
using UnityEngine;

public class SuckedState : BaseState
{
    private Enemy _enemy;

    float tickSpeed;
    float maxTime;
    int damagePerTick;

    public SuckedState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
    }
    public override void EnterState()
    {
        _enemy.enemyAnim.SetTrigger("GettingPulled");
        _enemy.agent.isStopped = true;
        _enemy.rb.isKinematic = false;
        maxTime = 3f;
        tickSpeed = 0.25f;
        damagePerTick = 3;
    }

    public override Type ExecuteState()
    {
        if(maxTime > 0f)
        {
            tickSpeed -= Time.deltaTime;
            if(tickSpeed <= 0f)
            {
                _enemy.TakeDamage(damagePerTick);
                tickSpeed = 0.25f;
            }
        }
        if (!_enemy.isInGravity)
        {
            _enemy.rb.isKinematic = true;
            _enemy.agent.isStopped = false;
            
            if (_enemy.enemyType == Enemy.EnemyType.NANNY)
            {
                return typeof(NannyIdleState);
            }
            else
            {
                return typeof(IdleState);
            }
        }
        return null; 
    }
}
