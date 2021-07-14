using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyAttackState : EnemyState
{
    List<Floor> nodes;
    float nextDamageEvent ;
    public EnemyAttackState(EnemyController character)
    {
        actor = character;
    }
    public override void Tick()
    {
        if (actor.ActionsLeft>0&&!actor.isAttacking)
        {
            if (Time.time >= nextDamageEvent)
            {
                nextDamageEvent = Time.time + actor.attackRate;
                actor.AttackTarget = nodes.First().tile.Ocupant;
                actor.body.transform.LookAt(actor.AttackTarget.transform);
                actor.Attack();
            }
            else
            {
                nextDamageEvent = Time.time + actor.attackRate;
            }
        }
        else
        {
            actor.turnEnd();
        }
        
    }
    public override void OnStateEnter()
    {
        nodes = actor.GetAttackableNodes(actor.CurrentNode, actor.AttackRange);
    }

    public override void OnStateExit()
    {
        actor.AttackTarget = null;
    }
}
