using System.Collections;
using System.Collections.Generic;
using util;
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit)
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
