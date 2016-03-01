using UnityEngine;
using System.Collections.Generic;

public abstract class Pathfinder
{
    // TODO: Add reference to map
    protected Map map;

    // Updates the reachable array with the costs of movement to each space on the map for the last unit to request it
    public abstract void updateReachable(Unit u);

    // Finds a valid path from point a to point b for the given Unit
    List<Point> getPath(Unit u, Point a, Point b)
    {
        return null; // TODO: Implement (Move using reachable from the destination back to origin by looking for adjacent tiles with lower values and choosing whichever)
    }
}
