using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Floor : MonoBehaviour
{
    public MeshRenderer Indicator;
    public BoxCollider center;
    [SerializeField]
    private List<Floor> neighboursFinal = new List<Floor>();
    [SerializeField]
    private float distance;
    private int cost;
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

        center = GetComponentInChildren<BoxCollider>();       
    }
    public List<Floor> getNeighbours()
    {
        return neighboursFinal;
    }
    public void Walkeable()
    {
        Indicator.material.color = Color.green;
    }
    public void isCurrent()
    {
        Indicator.material.color = Color.yellow;
    }
    private void unselected()
    {
        Indicator.enabled = false;
        Indicator.material.color = Color.white;
    }
    private void unavailable()
    {
        Indicator.material.color = Color.red;
    }
    private void selected()
    {
        Indicator.enabled = true;
        Indicator.material.color = Color.blue;
    }
    private void OnDrawGizmos()
    {
        foreach (var item in neighboursFinal)
        {            
            Gizmos.DrawLine(transform.position, item.transform.position);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            unselected();
        }
    }
}
