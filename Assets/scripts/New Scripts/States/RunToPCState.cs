using System;
using UnityEngine;
using UnityEngine.AI;

public class RunToPCState : BaseState
{
    private Enemy _enemy;
    Transform pc;

    public RunToPCState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;

        pc = _enemy.pc.transform;

    }

    public override void EnterState()
    {
        _enemy.ResetAttack();
        _enemy.agent.isStopped = false;
        _enemy.agent.updateRotation = true;
        _enemy.isWeaponFiringDone = true;
    }


    public override Type ExecuteState()
    {
        float distanceFromPC = CalculateDistance(pc);

        MoveTowardsPlayer();

        if (distanceFromPC <= _enemy.enemyData.attackRange)
        {
            switch (_enemy.enemyType)
            {
                case (Enemy.EnemyType.DRUNKENSEPOY):
                    return typeof(SepoyAttackState);
                    
                case (Enemy.EnemyType.BUTCHER):
                    return typeof(ButcherAttackState);
                    
                case (Enemy.EnemyType.SHADOW):
                    return typeof(ShadowAttackState);
            }
            
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

