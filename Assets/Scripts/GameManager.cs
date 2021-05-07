using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Entity>  Units;
    [SerializeField]
    List<Entity> orderedUnits;
    [SerializeField]
    Entity current;

    private void Start()
    {
        Units = FindObjectsOfType<Entity>().ToList();
        orderedUnits = arrangeUnitsBySpeed();
        TurnBegin(orderedUnits.First());
    }
    List<Entity> arrangeUnitsBySpeed()
    {
        return Units.OrderByDescending(x => x.Speed).ToList();
    }
    public void TurnBegin(Entity next)
    {
        current = next;
    }
    public void TurnEnd()
    {

    }
}
