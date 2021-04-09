using System.Collections;
using System.Collections.Generic;
using util;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
public class CharacterStateMove : CharacterState
{
    List<Floor> WalkeableNodes;
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        WalkeableNodes = actor.CurrentNode.Descendants(actor.Speed);
        foreach (var item in WalkeableNodes)
        {
            item.MakeFloorWalkeable();  
        }
    }  
    public override void Tick()
    {
        
    }
}
