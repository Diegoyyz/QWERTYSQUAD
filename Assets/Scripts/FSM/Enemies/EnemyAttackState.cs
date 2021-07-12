using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyController character)
    {
        actor = character;
    }
    public override void Tick()
    {
       
    }
    public override void OnStateEnter()
    {
        var nodes = actor.GetAttackableNodes(actor.CurrentNode,actor.AttackRange);
        if (nodes.Count>0&&actor.ActionsLeft>0)
        {
            actor.AttackTarget = nodes.First().tile.Ocupant;
            actor.body.transform.LookAt(actor.AttackTarget.transform);
            actor.Attack();           
        }       
    }  
    public override void OnStateExit()
    {
        actor.AttackTarget = null;
    }

}
