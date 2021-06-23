using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectedState : EnemyState
{
    public EnemySelectedState(EnemyController character)
    {
        actor = character;
    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
    }
    public override void Tick()
    {
        if (true)
        {
            actor.changeState(0);
        }
    }
}
