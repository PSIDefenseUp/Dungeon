using UnityEngine;
using System.Collections.Generic;

public class GroundPathfinder : Pathfinder
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

        // Initialize queue of tiles to evaluate and their distances
        Queue<int[]> field = new Queue<int[]>();

        // Add current position to queue with a distance of 0
        field.Enqueue(new int[] { u.getPosition().x, u.getPosition().y, 0 });
        
        // Create trackers for current position and distance
        int[] current;
        int distance = 0;

        // Create variables to store the tile and unit on the surrounding spaces
        Tile currentTile;
        Unit otherUnit;

        // Process queue until empty
        while (field.Count > 0)
        {
            // Get tile from queue
            current = field.Dequeue();

            // Fetch current distance
            distance = current[2];

            // Add the current space to reachable
            reachable[current[0], current[1]] = distance;

            // NORTH
            if (map.contains(new Point(current[0], current[1] - 1)))
            {
                currentTile = map.getTile(new Point(current[0], current[1] - 1));
                otherUnit = map.getUnit(new Point(current[0], current[1] - 1));
                if (currentTile != null && !currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed && reachable[current[0], current[1] - 1] == -1)
                {
                    if (otherUnit == null || otherUnit.team == u.team)
                        field.Enqueue(new int[] { current[0], current[1] - 1, distance + currentTile.moveCost });
                }
            }

            // SOUTH
            if (map.contains(new Point(current[0], current[1] + 1)))
            {
                currentTile = map.getTile(new Point(current[0], current[1] + 1));
                otherUnit = map.getUnit(new Point(current[0], current[1] + 1));
                if (currentTile != null && !currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed && reachable[current[0], current[1] + 1] == -1)
                {
                    if (otherUnit == null || otherUnit.team == u.team)
                        field.Enqueue(new int[] { current[0], current[1] + 1, distance + currentTile.moveCost });
                }
            }

            // EAST
            if (map.contains(new Point(current[0] + 1, current[1])))
            {
                currentTile = map.getTile(new Point(current[0] + 1, current[1]));
                otherUnit = map.getUnit(new Point(current[0] + 1, current[1]));
                if (currentTile != null && !currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed && reachable[current[0] + 1, current[1]] == -1)
                {
                    if (otherUnit == null || otherUnit.team == u.team)
                        field.Enqueue(new int[] { current[0] + 1, current[1], distance + currentTile.moveCost });
                }
            }

            // WEST
            if (map.contains(new Point(current[0] - 1, current[1])))
            {
                currentTile = map.getTile(new Point(current[0] - 1, current[1]));
                otherUnit = map.getUnit(new Point(current[0] - 1, current[1]));
                if (currentTile != null && !currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed && reachable[current[0] - 1, current[1]] == -1)
                {
                    if (otherUnit == null || otherUnit.team == u.team)
                        field.Enqueue(new int[] { current[0] - 1, current[1], distance + currentTile.moveCost });
                }
            }
        }

        u.reachable = reachable;

        /* DEBUG PRINT REACHABLE
        
        string s = "";
        for(int y = 0; y < map.getBounds().height; y++)
        {
            for(int x = 0; x < map.getBounds().width; x++)
            {
                s += (reachable[x, y] >= 0 ? reachable[x, y] : 9) + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
        */
    }       
}
