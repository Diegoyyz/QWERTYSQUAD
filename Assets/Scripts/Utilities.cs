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
        public static List<Floor> Descendants(this Floor root, int Speed,Action<Floor> getTargetCallback, Action<Floor> getTemporalTargetCallback)
        {
            var start = root;
            int speedLeft = Speed;
            List<Floor> toReturn = new List<Floor>();
            while (speedLeft >0)
            {
                speedLeft--;
                foreach (var item in start.getNeighbours())
                {
                    //get a list of walkeable nodes
                    var vecinos = item.Descendants(speedLeft,getTargetCallback,getTemporalTargetCallback);
                    item.MakeFloorPath();
                    //start pathfinding on button sellect
                    EventTrigger.Entry entry = new                  EventTrigger.Entry();
                    entry.eventID =                                 EventTriggerType.PointerEnter;
                    entry.callback.AddListener((eventData) => { getTemporalTargetCallback(item); });
                    item.GetComponent<EventTrigger> ().triggers.Add(entry);
                    //add callback to selection
                    item.GetComponent<Button>().onClick.AddListener(delegate { getTargetCallback(item);});
                    toReturn.Concat(vecinos);
                }
            }
            return toReturn;
        }

      
    }
}
