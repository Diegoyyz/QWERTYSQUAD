using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace util
{
    static class Utilities
    {
        public static void Descendants(this Floor root, int Speed,Action<Floor> getTargetCallback)
        {
            var start = root;
            int speedLeft = Speed;
            while (speedLeft >0)
            {
                speedLeft--;
                foreach (var item in start.getNeighbours())
                {                    
                    item.Descendants(speedLeft,getTargetCallback);
                    item.MakeFloorPath();
                    //start pathfinding on button sellect
                    EventTrigger.Entry entry = new  EventTrigger.Entry();
                    entry.eventID =                                 EventTriggerType.PointerEnter;                   
                    item.GetComponent<EventTrigger> ().triggers.Add(entry);
                    //add callback to selection
                    item.GetComponent<Button>().onClick.AddListener(delegate { getTargetCallback(item);});
                }
            }
        }

      
    }
}
