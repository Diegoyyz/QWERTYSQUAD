using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
public class Floor : MonoBehaviour
{
    [SerializeField]
    private List<Floor> neighbours = new List<Floor>();
    public int gCost = 1;
    public int hCost;
    [SerializeField]
    private float distance;
    public delegate void makeWalkeable();
    public event makeWalkeable OnMakeWalkeable;
    public delegate void resetFloor();
    public event makeGoal OnMakeGoal;
    public delegate void makeGoal();
    public event resetFloor OnResetFloor;
    public delegate void makePAth();
    public event makeWalkeable OnMakePath;
    public delegate void makeAttackable();
    public event makeAttackable onMakeAttackable;
    public delegate void nullSelection();
    public event nullSelection onDeselect;
    public delegate void isOcupied();
    public event isOcupied onIsOcupied;
    public bool walkable;
    public Floor parent;
    [SerializeField]
    public FloorTile tile;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }       
    private void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            neighbours.Add(hit.collider.GetComponentInChildren<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit))
        {
            neighbours.Add(hit.collider.GetComponentInChildren<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit))
        {
            neighbours.Add(hit.collider.GetComponentInChildren<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit))
        {
            neighbours.Add(hit.collider.GetComponentInChildren<Floor>());
        }
    }
    public void MakeAttackable()
    {
        onMakeAttackable();
    }
    public void ResetFloor()
    {
        OnResetFloor();
    }
    public void ResetOcupied()
    {
        onIsOcupied();
    }
    public void MakeFloorGoal()
    {
        OnMakeGoal();
        walkable = true;
    }
    public void MakeFloorWalkeable()
    {
        OnMakeWalkeable();
        walkable = true;
    }
    public void MakeFloorPath()
    {
        OnMakePath();
    }
    public List<Floor> getNeighbours()
    {
        return neighbours;
    }
    private void OnDrawGizmos()
    {
        foreach (var item in neighbours)
        {
            if (item != null)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }

}
