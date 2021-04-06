using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Floor : MonoBehaviour
{ 
    public MeshRenderer Indicator;
    public Transform center;
    [SerializeField]
    private Floor[] neighbours;

    private void Awake()
    {
        Indicator = GetComponentInChildren<MeshRenderer>();
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
