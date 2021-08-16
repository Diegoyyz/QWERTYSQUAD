using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController character)
    {
        actor = character;
    }
    public override void Tick()
    {
        if (actor.CurrentNode.getNeighbours().Where(x =>
                                  x.tile.IsOcupied &&
                                  x.tile.Ocupant.team != actor.team).Count() > 0)
        {
            actor.changeState(3);
        }
    }
    public override void OnStateEnter()
    {
        
    }
    public override void OnStateExit()
    {

    }
}
