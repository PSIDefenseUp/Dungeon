using UnityEngine;
using System.Collections;

public class NoPathfinding : Pathfinder
{
    public override void updateReachable(Unit u)
    {
        // get reference to map
        Map map = GameObject.Find("GameManager").GetComponent<Game>().map;

        // Get map bounds (for width and height)
        Rect mapBounds = map.getBounds();

        // Set up reachable array
        int[,] reachable = new int[(int)mapBounds.width, (int)mapBounds.height];

        // Initialize all spaces to -1 (unreachable)
        for (int i = 0; i < mapBounds.height; i++)
        {
            for (int j = 0; j < mapBounds.width; j++)
            {
                reachable[j, i] = -1;
            }
        }

        u.reachable = reachable;
    }
}
