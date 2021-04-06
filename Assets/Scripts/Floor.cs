using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Floor : MonoBehaviour
{
    public MeshRenderer Indicator;
    public BoxCollider center;
    [SerializeField]
    private List<GameObject> neighboursFinal = new List<GameObject>();
    [SerializeField]
    private float distance;

    private void Awake()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        center = GetComponentInChildren<BoxCollider>();
        neighboursFinal = hitColliders.Select(x => x.gameObject).Where(x => x.gameObject.tag.Equals("Floor")&& x.gameObject!= this.gameObject).ToList();
    }
    private void Walkeable()
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
