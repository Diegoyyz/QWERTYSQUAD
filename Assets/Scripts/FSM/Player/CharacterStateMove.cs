using System.Collections;
using System.Collections.Generic;
using util;
using System.Linq;
using UnityEngine;
public class CharacterStateMove : CharacterState
{
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        var WalkeableNodes = actor.CurrentTile.Descendants(actor.Speed);
        foreach (var item in WalkeableNodes)
        {
            item.Walkeable();
        }
    }
    
    public override void Tick()
    {

    }
}
