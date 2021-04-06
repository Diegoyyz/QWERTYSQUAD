using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Floor : MonoBehaviour
{
    public MeshRenderer Indicator;
    public Transform center;
    [SerializeField]
    private List<GameObject> neighboursFinal = new List<GameObject>();
    [SerializeField]
    private float distance;

    private void Awake()
    {
        neighboursFinal = GameObject.FindGameObjectsWithTag("Floor").
            Where(x =>x.gameObject != this.gameObject && Vector3.Distance(this.transform.position, x.transform.position) < distance).ToList();
        Indicator.enabled = false;
    }
    private void Walkeable()
    {
        Indicator.material.color = Color.green;
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
}
