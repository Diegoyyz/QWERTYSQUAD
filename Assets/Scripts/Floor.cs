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
    public delegate void makeOcupied();
    public event makeWalkeable OnMakeOcupied;
    public delegate void makeUnWalkeable();
    public event makeWalkeable OnMakeUnWalkeable;
    public delegate void makeAttackable();
    public event makeAttackable onMakeAttackable;

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
    public void togleWalkeable()
    {
        walkable =!walkable;
    }
    private void Awake()
    {
        tile.OnTogleWalkeable += togleWalkeable;
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
        tile.IsOcupied = false;
    }
    public void MakeFloorWalkeable()
    {
        walkable = true;
        OnMakeWalkeable();
    }
    public void MakeFloorUnWalkeable()
    {
        OnMakeUnWalkeable();      
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
