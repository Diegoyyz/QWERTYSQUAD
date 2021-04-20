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

    List<Floor> path;
    Floor target;
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        actor.CurrentNode.Descendants(actor.SpeedLeft, GetTargetNode);       
    }
    public override void OnStateExit()
    {
      
    }
    public override void Tick()
    {
        if (actor.CurrentNode != actor.TargetNode)
        {
            FindPath(actor.CurrentNode,actor.TargetNode);  
        }      
    }   
    private void RetracePath(Floor startNode, Floor endNode)
    {
        List<Floor> rPath = new List<Floor>();
        Floor currentNode = endNode;
        if (path != null)
        {
            foreach (var item in path)
            {
                item.ResetFloor();
            }
        }
        while (currentNode != startNode)
        {
            rPath.Add(currentNode);
            currentNode = currentNode.parent;
            currentNode.MakeFloorWalkeable();
        }
        endNode.MakeFloorGoal();
        actor.okMove.transform.position = new Vector3(endNode.transform.position.x, actor.okMove.transform.position.y, endNode.transform.position.z);
        actor.okMove.gameObject.SetActive(true);
        rPath.Reverse();
        actor.SpeedLeft -= rPath.Count();
        path = rPath;

    }
    void FindPath(Floor start, Floor target)
    {
        Floor startNode = start;
        Floor targetNode = target;
        List<Floor> openSet = new List<Floor>();
        HashSet<Floor> closedSet = new HashSet<Floor>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Floor node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }
            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }
            foreach (Floor item in node.getNeighbours())
            {
                if (!item.walkable || closedSet.Contains(item))
                {
                    continue;
                }
                int newCostToNeighbour = node.gCost + GetDistance(node, item);
                if (newCostToNeighbour < item.gCost || !openSet.Contains(item))
                {
                    item.gCost = newCostToNeighbour;
                    item.hCost = GetDistance(item, targetNode);
                    item.parent = node;
                    if (!openSet.Contains(item))
                        openSet.Add(item);
                }
            }
        }
    }
    static int GetDistance(Floor nodeA, Floor nodeB)
    {
        if (nodeA!= null && nodeB != null)
        {       
        int dstX = (int)Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
        int dstY = (int)Mathf.Abs(nodeA.transform.position.x - nodeA.transform.position.y);
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
        }
        return 0;
    }
    void GetTargetNode(Floor target)
    {
        actor.TargetNode = target;
    }
    void GetTemporalTargetNode(Floor target)
    {
       
    }
}
