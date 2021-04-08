using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Floor : MonoBehaviour
{
    [SerializeField]
    private List<Floor> neighboursFinal = new List<Floor>();
    [SerializeField]
    private float distance;
    public delegate void makeWalkeable();
    public event makeWalkeable OnMakeWalkeable;

    private void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
          neighboursFinal.Add(hit.collider.GetComponent<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit))
        {
            neighboursFinal.Add(hit.collider.GetComponent<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit))
        {
            neighboursFinal.Add(hit.collider.GetComponent<Floor>());
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit))
        {
            neighboursFinal.Add(hit.collider.GetComponent<Floor>());
        }
    }      
    public void MakeFloorWalkeable()
    {
        OnMakeWalkeable();
    }
    public List<Floor> getNeighbours()
    {
        return neighboursFinal;
    }
   
    private void OnDrawGizmos()
    {
        foreach (var item in neighboursFinal)
        {            
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }  
}
