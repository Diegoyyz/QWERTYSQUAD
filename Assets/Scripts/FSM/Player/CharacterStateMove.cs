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
    List<Floor> path;

    Floor target;
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        WalkeableNodes = actor.CurrentNode.Descendants(actor.Speed, GetTargetNode, GetTemporalTargetNode);
        foreach (var item in WalkeableNodes)
        {
            item.MakeFloorWalkeable();

        }
    }  
    public override void Tick()
    {
        if (actor.CurrentNode != actor.TargetNode)
        {
            FindPath(actor.CurrentNode, WalkeableNodes, actor.TargetNode);
        }
        if (path != null)
        {
            Debug.Log("found");
            if (path.Contains(actor.TargetNode))
            {
                foreach (var item in path)
                {
                    item.MakeFloorPath();
                }
            }           
        }
    }
    private  void RetracePath(Floor startNode, Floor endNode)
    {
        Floor currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();       
    }
    private void FindPath(Floor StartNode, List<Floor> Nodes, Floor targetNodes)
    {
        Floor startNode = StartNode;
        Floor targetNode = targetNodes;
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

            foreach (Floor neighbour in Nodes)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                        neighbour.MakeFloorPath();
                    }
                }
            }
        }
    }

    static int GetDistance(Floor nodeA, Floor nodeB)
    {
        int dstX = (int)Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
        int dstY = (int)Mathf.Abs(nodeA.transform.position.x - nodeA.transform.position.y);
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
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
