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
        GetAttackableNodes(actor.CurrentNode, actor.AttackRange);
    }
    public override void OnStateExit()
    {
    }
    public static void GetAttackableNodes(Floor root, int Speed)
    {
        var start = root;
        int speedLeft = Speed;
        while (speedLeft > 0)
        {
            speedLeft--;
            foreach (var item in start.getNeighbours())
            {                
                if (item.tile.Ocupant!= null)
                {
                    item.MakeAttackable();
                    GetAttackableNodes(item, speedLeft);
                }
               
            }
        }
    }
}
