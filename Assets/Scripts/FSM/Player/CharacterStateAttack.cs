using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterStateAttack : CharacterState
{
    List<Floor> neighbours= new List<Floor>();
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
                if (hit.collider.gameObject.tag=="Entity"&&hit.collider.GetComponent<Entity>().team != actor.team)
                {
                    Target = hit.collider.GetComponent<Entity>();
                    actor.okAttack.transform.position = new Vector3(hit.collider.GetComponent<Entity>().transform.position.x-1, actor.okMove.transform.position.y+1, hit.collider.GetComponent<Entity>().transform.position.z-1);
                    actor.body.transform.LookAt(hit.collider.GetComponent<Entity>().transform);
                    actor.toggleOkAttack();
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
        foreach (var item in neighbours)
        {
            if (!item.tile.IsOcupied)
            {
                item.ResetFloor();
            }
            if (item.tile.IsOcupied)
            {
                item.ResetOcupied();
            }
        }
    }
    public void GetAttackableNodes(Floor root, int Speed)
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
                    neighbours.Add(item);
                    GetAttackableNodes(item, speedLeft);
                }
               
            }
        }
    }
}
