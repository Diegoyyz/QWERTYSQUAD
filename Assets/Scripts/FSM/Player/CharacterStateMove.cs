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
    bool speedRested;
    Floor target;
    public CharacterStateMove(CharacterController character)
    {
        actor = character;        
    }
    public override void OnStateEnter()
    {        
        actor.toggleController();
        speedRested = false;
        actor.ResetStats();
        Descendants(actor.CurrentNode,actor.SpeedLeft, GetTargetNode);       
    }
    public override void OnStateExit()
    {

        if (path != null)
        {
            actor.SpeedLeft -= path.Count();
            actor.MoveToTarget(path);
        }
        foreach (var item in WalkeableNodes)
        {
            item.ResetFloor();
        }

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
                item.MakeFloorPath();               
            }
        }
        while (currentNode != startNode)
        {
            rPath.Add(currentNode);
            currentNode = currentNode.parent;
            currentNode.MakeFloorWalkeable();
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
                    if (!openSet.Contains(item))
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
        while (speedLeft > 0)
        {
            speedLeft--;
            foreach (var item in start.getNeighbours())
            {
                Descendants(item,speedLeft, getTargetCallback);
                item.MakeFloorPath();
                //start pathfinding on button sellect
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                item.GetComponent<EventTrigger>().triggers.Add(entry);
                //add callback to selection
                item.GetComponent<Button>().onClick.AddListener(delegate { getTargetCallback(item); });
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
