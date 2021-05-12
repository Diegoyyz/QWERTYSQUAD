using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class EnemyMoveState : CharacterState
{
    List<Floor> path;
    List<Floor> WalkeableNodes = new List<Floor>();
    bool speedRested;
    Floor target;
    public EnemyMoveState(Entity character)
    {
        actor = character;
    }
    public override void Tick()
    {
        throw new System.NotImplementedException();
    }
    public override void OnStateEnter()
    {
        var tiles = GameObject.FindObjectsOfType<Floor>();
        Descendants(actor.CurrentNode, actor.ActionsLeft, GetTargetNode);
        FindPath(actor.CurrentNode, target);
        var partialpath = path.Intersect(WalkeableNodes);
    }
    public override void OnStateExit()
    {
        //ejecutar corrutina para mover enemigo 
    }
    private void RetracePath(Floor startNode, Floor endNode)
    {
        List<Floor> rPath = new List<Floor>();
        Floor currentNode = endNode;
        if (path != null)
        {
            foreach (var item in path)
            {
                if (!item.tile.IsOcupied)
                {
                    item.MakeFloorPath();
                }
            }
        }
        while (currentNode != startNode)
        {
            rPath.Add(currentNode);
            currentNode = currentNode.parent;
            if (!currentNode.tile.IsOcupied)
            {
                currentNode.MakeFloorWalkeable();
            }
        }
        rPath.Add(actor.CurrentNode);
        endNode.MakeFloorGoal();
        actor.okMove.transform.position = new Vector3(endNode.transform.position.x, actor.okMove.transform.position.y, endNode.transform.position.z);
        actor.okMove.gameObject.SetActive(true);
        rPath.Reverse();
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
                    if (!openSet.Contains(item) && !item.tile.IsOcupied)
                        openSet.Add(item);
                }

            }
        }
    }
    public void Descendants(Floor root, int Actions, Action<Floor> getTargetCallback)
    {
        WalkeableNodes.Add(root);
        var start = root;
        int speedLeft = Actions;
        while (speedLeft > 0)
        {
            speedLeft--;
            foreach (var item in start.getNeighbours())
            {
                Descendants(item, speedLeft, getTargetCallback);
            }
        }
    }
    static int GetDistance(Floor nodeA, Floor nodeB)
    {
        if (nodeA != null && nodeB != null)
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
}
