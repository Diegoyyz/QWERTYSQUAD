using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Entity>  Units;
    [SerializeField]
    List<Entity> orderedUnits;
    [SerializeField]
    List<Entity> playerUnits= new List<Entity>();
    [SerializeField]
    List<Entity> iAUnits = new List<Entity>();


    Entity current;
    [SerializeField]
    int TurnIndex;
    [SerializeField]
    Text actionsLeftTxt;
    bool Waiting;

    public void deleteEntity(Entity toDelete)
    {
        orderedUnits.Remove(toDelete);
        Units.Remove(toDelete);
        if (playerUnits.Contains(toDelete))
        {
            playerUnits.Remove(toDelete);
        }
        else if (iAUnits.Contains(toDelete))
        {
            iAUnits.Remove(toDelete);
        }
    }
    private void OnDestroy()
    {
        foreach (var item in Units)
        {
            item.onDeathEvent -= deleteEntity;
        }
    }
    private void Start()
    {
        TurnIndex = 0;
        Units = FindObjectsOfType<Entity>().ToList();      
        
        foreach (var item in Units)
        {
            item.onDeathEvent += deleteEntity;
            if (item.team == Entity.Teams.Blue)
            {
                playerUnits.Add(item);
            }
            else
            {
                iAUnits.Add(item);
            }
            item.onTurnEndsEvent += TurnEnd;
            item.onTurnStartsEvent += TurnStart;         
        }
        orderedUnits = arrangeUnitsBySpeed();
        Waiting = true;
        StartCoroutine(DelayCorroutine());
    }
    IEnumerator DelayCorroutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        Waiting = false;
        TurnStart();
    }
    public void RemoveFromList()
    {

    }
    List<Entity> arrangeUnitsBySpeed()
    {
        return Units.OrderByDescending(x => x.Speed).ToList();
    }
    public void TurnStart()
    {
        current = orderedUnits[TurnIndex];
        current.turnStart();
    }
    private void Update()
    {
        if (!Waiting)
        {        
        if (actionsLeftTxt != null)
        {
            actionsLeftTxt.text = "Actions Left: "+current.ActionsLeft ;
        }
        if (current.ActionsLeft<= 0)
        {
            actionsLeftTxt.color = Color.red;
        }
        else
        {
            actionsLeftTxt.color = Color.green;
        }
        }
        if (iAUnits.Count <=0)
        {
            Debug.Log("Ganaste fierita");
        }
        else if (playerUnits.Count <= 0)
        {
            Debug.Log("Perdiste Reynolds");
        }
    }
    void selecUnit()
    {
        while (orderedUnits[TurnIndex].isDead)
        {
            TurnIndex++;
        }
        current = orderedUnits[TurnIndex];        
    }
    public void TurnEnd()
    {
         TurnIndex++;
        current.turnEnd();   
        orderedUnits = arrangeUnitsBySpeed();
        if (TurnIndex >orderedUnits.Count()-1)
        {
            TurnIndex = 0;
        }
        selecUnit();
        TurnStart();
    }
}
