using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Entity>  Units;
    [SerializeField]
    List<Entity> orderedUnits;
    Entity current;
    [SerializeField]
    int TurnIndex;
    [SerializeField]
    Text actionsLeftTxt;

    private void Start()
    {
        TurnIndex = 0;
        Units = FindObjectsOfType<Entity>().ToList();
        foreach (var item in Units)
        {
            item.onTurnEndsEvent += TurnEnd;
            item.onTurnStartsEvent += TurnStart;
        }
        orderedUnits = arrangeUnitsBySpeed();     
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
