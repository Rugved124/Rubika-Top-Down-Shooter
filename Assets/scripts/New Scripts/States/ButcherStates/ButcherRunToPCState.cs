using System;
using UnityEngine;

public class ButcherRunToPCState : BaseState
{
    private Enemy _enemy;
    Transform pc;

    public ButcherRunToPCState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

        pc = _enemy.pc.transform;

    }

    public override void EnterState()
    {
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
    }


    public override Type ExecuteState()
    {
        float distanceFromPC = CalculateDistance(pc);

        MoveTowardsPlayer();

        if (distanceFromPC < _enemy.enemyData.attackRange)
        {
            return typeof(ButcherAttackState);
        }

        return null;
    }
    private void MoveTowardsPlayer()
    {
        _enemy.agent.SetDestination(pc.position);
    }


    float CalculateDistance(Transform objTransform)
    {
        float distanceFromObj = Vector3.Distance(_enemy.transform.position, objTransform.position);

        return distanceFromObj;
    }
}
