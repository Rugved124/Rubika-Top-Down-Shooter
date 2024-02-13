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
    }

    public override Type ExecuteState()
    {
        if (_enemy.hpPercent <= 20 && _enemy.canRunAway)
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
            }
            if(_enemy.enemyType == Enemy.EnemyType.SHADOW && distanceFromPC <= _enemy.enemyData.attackRange / 2)
            {
                return typeof(ShadowAttackState);
            }
        }

        return null;
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }

    void Think()
    {

    }
}
