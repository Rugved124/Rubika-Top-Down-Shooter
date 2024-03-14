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
        _enemy.enemyAnim.SetTrigger("RunState");
        if(_enemy.enemyType != Enemy.EnemyType.NANNY)
        {
            _enemy.ResetAttack();
        }

        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.isWeaponFiringDone = true;
        angle = UnityEngine.Random.Range(-120f, 120f);
        stopPoint = (pc.position - transform.position).normalized * (_enemy.enemyData.attackRange - 2f);
        stopPoint = Quaternion.AngleAxis(angle, Vector3.up) * stopPoint;
    }


    public override Type ExecuteState()
    {
        if (_enemy.isInGravity)
        {
            return typeof(SuckedState);
        }
        if (_enemy.hpPercent <= 20 && !_enemy.isShielded && _enemy.canRunAway)
        {
            return typeof (RunAwayState);
        }
        if (pc != null)
        {
            float distanceFromPC = CalculateDistance(pc);
            MoveTowardsPlayer();
            if (_enemy.enemyType == Enemy.EnemyType.NANNY)
            {

                if (_enemy.agent.remainingDistance <= 1f || _enemy.lowHpEnemy.Count > 0)
                {
                    return typeof(NannyIdleState);
                }
            }
            else
            {
                if (_enemy.agent.remainingDistance <= 1f)
                {
                    return typeof(IdleState);
                }
            }
        }
        return null;
    }
    private void MoveTowardsPlayer()
    {
        if(_enemy.enemyType == Enemy.EnemyType.DRUNKENSEPOY)
        {
            _enemy.agent.SetDestination(_enemy.surroundPos);
        }
        else
        {
            _enemy.agent.SetDestination(stopPoint + pc.position);
        }
       
    }
    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);
        return distanceFromObj;
    }
} 

