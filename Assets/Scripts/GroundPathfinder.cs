using UnityEngine;
using System.Collections.Generic;

public class GroundPathfinder : Pathfinder
{
    public override void updateReachable(Unit u)
    {
        // TODO: REWRITE THIS BUGGY PIECE OF SHIT 

        // Get map bounds (for width and height)
        Rect mapBounds = this.map.getBounds();

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
        field.Enqueue(new int[] { u.getPosition().x, u.getPosition().y, 0});
        reachable[u.getPosition().x, u.getPosition().y] = 0;

        // Create trackers for current position and distance
        int[] current;
        int distance = 0;

        // Create variables to store the tile and unit on the surrounding spaces
        Tile currentTile;
        Unit otherUnit;
        
        // Process queue until empty
        while(field.Count > 0)
        {
            // Get tile from queue
            current = field.Dequeue();

            // Fetch current distance
            distance = current[2];

            // TODO: Below, check to see if the locations are within the bounds of the map -- THIS WILL NOT WORK OTHERWISE
   
            // NORTH
            currentTile = map.getTile(new Point(current[0], current[1] - 1));
            otherUnit = map.getUnit(new Point(current[0], current[1] - 1));
            if (!currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed)
            {
                if (otherUnit == null || otherUnit.team == u.team)
                    field.Enqueue(new int[] { current[0], current[1] - 1, distance + currentTile.moveCost });

                reachable[current[0], current[1] - 1] = distance;
            }

            // SOUTH
            currentTile = map.getTile(new Point(current[0], current[1] + 1));
            otherUnit = map.getUnit(new Point(current[0], current[1] + 1));
            if (!currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed)
            {
                if (otherUnit == null || otherUnit.team == u.team)
                    field.Enqueue(new int[] { current[0], current[1] + 1, distance + currentTile.moveCost });

                reachable[current[0], current[1] + 1] = distance;
            }

            // EAST
            currentTile = map.getTile(new Point(current[0] + 1, current[1]));
            otherUnit = map.getUnit(new Point(current[0] + 1, current[1]));
            if (!currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed)
            {
                if (otherUnit == null || otherUnit.team == u.team)
                    field.Enqueue(new int[] { current[0] + 1, current[1], distance + currentTile.moveCost });

                reachable[current[0] + 1, current[1]] = distance;
            }

            // WEST
            currentTile = map.getTile(new Point(current[0] - 1, current[1]));
            otherUnit = map.getUnit(new Point(current[0] - 1, current[1]));
            if (!currentTile.solid && distance + currentTile.moveCost <= u.moveSpeed)
            {
                if (otherUnit == null || otherUnit.team == u.team)
                    field.Enqueue(new int[] { current[0] - 1, current[1], distance + currentTile.moveCost });

                reachable[current[0] - 1, current[1]] = distance;
            }                        
        }

        u.setReachable(reachable);
    }    
}
