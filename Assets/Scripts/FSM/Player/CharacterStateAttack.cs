using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterStateAttack : CharacterState
{
    List<Floor> neighbours;

    public CharacterStateAttack(CharacterController character)
    {
        actor = character;
    }
    public override void Tick()
    {

    }
    public override void OnStateEnter()
    {
        neighbours = actor.CurrentNode.getNeighbours();
        foreach (var item in neighbours)
        {            
                item.MakeAttackable();            
        }
    }
    public override void OnStateExit()
    {
    }
}
