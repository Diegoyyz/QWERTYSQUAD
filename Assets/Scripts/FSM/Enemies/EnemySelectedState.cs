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
        actor.ResetStats();
    }
    public override void Tick()
    {
       
    }
}
