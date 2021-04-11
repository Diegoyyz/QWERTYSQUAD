using System.Collections;
using System.Collections.Generic;
using util;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
public class CharacterStateMove : CharacterState
{
    List<Floor> WalkeableNodes;
    Floor target;
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        WalkeableNodes = actor.CurrentNode.Descendants(actor.Speed, GetTargetNode, GetTemporalTargetNode);
       
    }  
    public override void Tick()
    {
        if (actor.TargetNode !=null)
        {
          actor.CurrentNode.FindPath(WalkeableNodes,actor.TargetNode);
        }
    }  
    void GetTargetNode(Floor target)
    {
        actor.TargetNode = target;
    }
    void GetTemporalTargetNode(Floor target)
    {
        actor.TargetNode = target;
    }
}
