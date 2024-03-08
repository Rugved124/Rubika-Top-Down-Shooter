using System;
using UnityEngine;

public class IdleState : BaseState
{
    private Enemy _enemy;

    Quaternion startAngle = Quaternion.AngleAxis(0, Vector3.up);

    bool rollDiceShadow;
    float chance = 0;
    public IdleState(Enemy enemy):base(enemy.gameObject)
    {
        _enemy = enemy;

        startAngle = Quaternion.AngleAxis(-_enemy.enemyData.visionConeAngle/2f, Vector3.up);
    }
    public override void EnterState()
    {
        rollDiceShadow = true;
        _enemy.isCharging = false;
        _enemy.enemyAnim.SetTrigger("IdleState");
    }

    public override Type ExecuteState()
    {
        if (_enemy.hpPercent <= 20 && _enemy.canRunAway && !_enemy.canShield)
        {
            return typeof(RunAwayState);
        }

        float distanceFromPC = CalculateDistance(_enemy.pc.transform);

        if (distanceFromPC > _enemy.enemyData.attackRange)
        {
            return typeof(RunToPCState);
        }
        else if (distanceFromPC <= _enemy.enemyData.attackRange)
        {
            switch (_enemy.enemyType)
            {
                case Enemy.EnemyType.DRUNKENSEPOY:
                    return typeof(SepoyAttackState);

                case Enemy.EnemyType.BUTCHER:
                    return typeof(ButcherAttackState);
                case Enemy.EnemyType.SHADOW:
                    return typeof(ShadowAttackState);
            }
        }
        if (_enemy.isInGravity)
        {
            return typeof(SuckedState);
        }
        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
