using UnityEngine;
using System.Collections.Generic;

public abstract class Pathfinder
{
    // TODO: Add reference to map to obtain size

    // Updates the reachable array with the costs of movement to each space on the map for the last unit to request it
    public void updateReachable(Unit u)
    {
            
    }

    // Finds a valid path from point a to point b for the given Unit
    List<Point> getPath(Unit u, Point a, Point b)
    {
        return null; // TODO: Implement
    }
}
