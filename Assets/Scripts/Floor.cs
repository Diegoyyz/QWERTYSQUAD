using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Floor : MonoBehaviour
{ 
    public MeshRenderer mat;
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>();
        mat.enabled = false;            
    }    
    private void OnMouseDown()
    {
        mat.material.color = Color.green;
    }
    private void OnMouseExit()
    {
        mat.enabled = false;
        mat.material.color = Color.white;
    }
    private void OnMouseEnter()
    {
        mat.enabled = true;        
        mat.material.color = Color.blue;
    }
}
