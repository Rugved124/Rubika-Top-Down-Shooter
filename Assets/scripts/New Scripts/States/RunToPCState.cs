using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class RunToPCState : BaseState
{
    private Enemy _enemy;
    Transform pc;
    float angle;
    Vector3 stopPoint;
    public RunToPCState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

        pc = _enemy.pc.transform;

    }

    public override void EnterState()
    {
        if(_enemy.enemyType != Enemy.EnemyType.NANNY)
        {
            _enemy.ResetAttack();
        }

        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.isWeaponFiringDone = true;
        angle = UnityEngine.Random.Range(-60f, 60f);
        stopPoint = (pc.position - transform.position).normalized * (_enemy.enemyData.attackRange - 2f);
        stopPoint = Quaternion.AngleAxis(angle, Vector3.up) * stopPoint;
    }


    public override Type ExecuteState()
    {
        
        if(_enemy.hpPercent <= 20)
        {
            return typeof (RunAwayState);
        }
        if (pc != null)
        {
            float distanceFromPC = CalculateDistance(pc);

            if(_enemy.enemyType == Enemy.EnemyType.NANNY)
            { 

                if ( distanceFromPC <= _enemy.enemyData.attackRange / 3 || _enemy.lowHpEnemy.Count > 0)
                {
                    return typeof(NannyIdleState);
                }
            }
            MoveTowardsPlayer();

            if (_enemy.agent.remainingDistance <= 1f)
            {
                switch (_enemy.enemyType)
               {
                    case (Enemy.EnemyType.DRUNKENSEPOY):
                        return typeof(SepoyAttackState);

                    case (Enemy.EnemyType.BUTCHER):
                        return typeof(ButcherAttackState);

                    case (Enemy.EnemyType.SHADOW):
                        return typeof(ShadowAttackState);
                    case (Enemy.EnemyType.NANNY):
                        return typeof(NannyAttackState);
                }

            }
        }

      
        return null;
    }
    private void MoveTowardsPlayer()
    {
        _enemy.agent.SetDestination(pc.position - stopPoint);
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);
        return distanceFromObj;
    }
} 

