using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        orderedUnits = arrangeUnitsBySpeed();        
        TurnBegin();        
    }
    public void RemoveFromList()
    {

    }
    List<Entity> arrangeUnitsBySpeed()
    {
        return Units.OrderByDescending(x => x.Speed).ToList();
    }
    public void TurnBegin()
    {
        current = orderedUnits[TurnIndex];
        current.SetState(new CharacterStateSelected(current.GetComponent<CharacterController>()));
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
        current.controllerOff();
        current.SetState(new CharacterStateIdle(current.GetComponent<CharacterController>()));       
        orderedUnits = arrangeUnitsBySpeed();
        if (TurnIndex >orderedUnits.Count()-1)
        {
            TurnIndex = 0;
        }
        selecUnit();
        TurnBegin();
    }
}
