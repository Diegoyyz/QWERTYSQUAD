using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
public class CharacterStateMove : CharacterState
{
    List<Floor> path;
    List<Floor> WalkeableNodes = new List<Floor>();   
    public CharacterStateMove(CharacterController character)
    {
        actor = character;
    }
    public override void OnStateEnter()
    {
        Descendants(actor.CurrentNode, actor.ActionsLeft, GetTargetNode);
    }
    public override void OnStateExit()
    {
        foreach (var item in WalkeableNodes)
        {
            if (!item.tile.IsOcupied)
            {
                item.ResetFloor();
            }
        }
    }
    public override void Tick()
    {
        if (path != null&& !actor.onTheMoove)
        {
            actor.anim.SetBool("Walk Forward", false);
            actor.MoveToTargetNode(path);
        }
        else
        {
            FindPath(actor.CurrentNode, actor.TargetNode);
        }
        if (!actor.onTheMoove&&actor.CurrentNode==actor.TargetNode)
        {
            actor.anim.SetBool("Walk Forward", false);
            if (actor.ActionsLeft>0)
            {
                actor.changeState(1);
            }
            else if (actor.ActionsLeft <= 0)
            {
                actor.changeState(0);
            }
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
    public void Descendants(Floor root, int Speed, Action<Floor> getTargetCallback)
    {
        WalkeableNodes.Add(root);
        var start = root;
        int speedLeft = Speed;
        if (speedLeft < 1)
            return;
        speedLeft--;
        foreach (var item in start.getNeighbours())
        {
            Descendants(item, speedLeft, getTargetCallback);
            if (!item.tile.IsOcupied)
            {
                item.MakeFloorPath();
            }
            //start pathfinding on button sellect
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            item.GetComponent<EventTrigger>().triggers.Add(entry);
            //add callback to selection
            item.GetComponent<Button>().onClick.AddListener(delegate { getTargetCallback(item); });
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
