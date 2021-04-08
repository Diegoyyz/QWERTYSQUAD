using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace util
{
    static class Utilities
    {
        public static List<Floor> Descendants(this Floor root, int Speed)
        {
            var start = root;
            int speedLeft = Speed;
            List<Floor> toReturn = new List<Floor>();
            while (speedLeft >0)
            {
                foreach (var item in start.getNeighbours())
                {
                    speedLeft--;
                    var vecinos = item.GetComponent<Floor>().Descendants(speedLeft);
                    item.GetComponent<Floor>().MakeFloorWalkeable();
                    toReturn.Concat(vecinos);                   
                }
            }
            return toReturn;
        }
    }
}
