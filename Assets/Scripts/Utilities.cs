using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace util
{
    static class Utilities
    {
        public static List<Floor> Descendants(this Floor root, int Speed,Action<Floor> getTargetCallback, Action<Floor> getTemporalTargetCallback)
        {
            var start = root;
            int speedLeft = Speed;
            List<Floor> toReturn = new List<Floor>();
            while (speedLeft >0)
            {
                speedLeft--;
                foreach (var item in start.getNeighbours())
                {
                    //get a list of walkeable nodes
                    var vecinos = item.Descendants(speedLeft,getTargetCallback,getTemporalTargetCallback);
                    item.MakeFloorWalkeable();
                    //start pathfinding on button sellect
                    EventTrigger.Entry entry = new                  EventTrigger.Entry();
                    entry.eventID =                                 EventTriggerType.PointerEnter;
                    entry.callback.AddListener((eventData) => { getTemporalTargetCallback(item); });
                    item.GetComponent<EventTrigger> ().triggers.Add(entry);
                    //add callback to selection
                    item.GetComponent<Button>().onClick.AddListener(delegate { getTargetCallback(item);});
                    toReturn.Concat(vecinos);
                }
            }
            return toReturn;
        }

        public static void FindPath(this Floor StartNode, List<Floor> Nodes, Floor targetNodes )
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
                    return ;
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
                            Debug.Log("fafafa");
                        }
                    }
                }
            }
        }

        static List<Floor> RetracePath(Floor startNode, Floor endNode)
        {
            List<Floor> path = new List<Floor>();
            Floor currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            return path;

        }

        static int GetDistance(Floor nodeA, Floor nodeB)
        {
            int dstX = (int)Mathf.Abs(nodeA.transform.position.x - nodeB.transform.position.x);
            int dstY = (int)Mathf.Abs(nodeA.transform.position.x - nodeA.transform.position.y);
            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
