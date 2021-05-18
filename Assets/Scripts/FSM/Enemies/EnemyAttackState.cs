using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAttackState : CharacterState
{
    public EnemyAttackState(Entity character)
    {
        actor = character;
    }
    public override void Tick()
    {
        throw new System.NotImplementedException();
    }
    public override void OnStateEnter()
    {
        var nodes = actor.GetAttackableNodes(actor.CurrentNode,actor.AttackRange);
        if (nodes.Count>0)
        {
            actor.AttackTarget = nodes.First().tile.Ocupant;
            actor.body.transform.LookAt(actor.AttackTarget.transform);
            actor.Attack();
        }
        actor.changeState(2);
    }  
    public override void OnStateExit()
    {

    }
}
