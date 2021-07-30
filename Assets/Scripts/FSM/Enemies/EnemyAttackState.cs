using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyAttackState : EnemyState
{
    List<Floor> nodes;
    float nextFire;
    public EnemyAttackState(EnemyController character)
    {
        actor = character;
    }
    public override void Tick()
    {
        if (Time.time>nextFire&&actor.ActionsLeft>0&&!actor.isAttacking)
        {
            nextFire = Time.time + actor.attackRate;
            actor.AttackTarget = nodes.First().tile.Ocupant;
            if (actor.AttackTarget!= null)
            {
                actor.body.transform.LookAt(actor.AttackTarget.transform);
                actor.Attack();
            }
            else
            {
                actor.changeState(2);
            }
        }
        else if (actor.ActionsLeft <= 0)
        {
            actor.changeState(0);
        }
    }
    public override void OnStateEnter()
    {
        nextFire = 0.0f;
        nodes = actor.GetAttackableNodes(actor.CurrentNode, actor.AttackRange);
    }
    public override void OnStateExit()
    {
        actor.AttackTarget = null;
    }
}
