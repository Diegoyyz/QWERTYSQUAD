using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterStateAttack : CharacterState
{
    List<Floor> neighbours;
    Entity Target;
    public CharacterStateAttack(CharacterController character)
    {
        actor = character;
    }
    public override void Tick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<Entity>().team != actor.team)
                {
                    Target = hit.collider.GetComponent<Entity>();
                    actor.okAttack.transform.position = new Vector3(hit.collider.GetComponent<Entity>().transform.position.x, actor.okMove.transform.position.y, hit.collider.GetComponent<Entity>().transform.position.z);
                    actor.body.transform.LookAt(hit.collider.GetComponent<Entity>().transform);
                    actor.attackTarget = hit.collider.GetComponent<Entity>();


                }
            }
            actor.SetState(new CharacterStateIdle(actor));
        }
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
