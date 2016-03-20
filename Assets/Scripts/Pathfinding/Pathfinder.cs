using UnityEngine;
using System.Collections.Generic;

public abstract class Pathfinder : MonoBehaviour
{
    // Updates the reachable array with the costs of movement to each space on the map for the last unit to request it
    public abstract void updateReachable(Unit u);

    // Finds a valid path from point a to point b for the given Unit
    public void getPath(Unit u, Point p)
    {
        Map map = GameObject.Find("GameManager").GetComponent<Game>().map;
        u.path = new Stack<Tile>();

        // Add destination as last point in path
        u.path.Push(map.getTile(p));
        Debug.Log("Pathend: " + p.ToString());

        // TODO: REMOVE?
        if (u.pathfinder is NoPathfinding)
            return;

        // TODO: Until we are back at u's current location, move back from the dest 
        Tile next;  // Tile we will add to the queue at the end of the loop
        Tile other; // Current tile we are evaluating
        
        while(!(u.path.Peek().getPosition().x == u.getPosition().x && u.path.Peek().getPosition().y == u.getPosition().y))
        {
            next = null;

            // NORTH
            if (map.contains(new Point(u.path.Peek().getPosition().x, u.path.Peek().getPosition().y - 1)))
            {
                other = map.getTile(new Point(u.path.Peek().getPosition().x, u.path.Peek().getPosition().y - 1));

                if (other != null && u.reachable[other.getPosition().x, other.getPosition().y] >= 0)
                {
                    if (next == null || u.reachable[next.getPosition().x, next.getPosition().y] == -1 || u.reachable[other.getPosition().x, other.getPosition().y] <= u.reachable[next.getPosition().x, next.getPosition().y])
                        next = other;
                }
            }

            // SOUTH
            if (map.contains(new Point(u.path.Peek().getPosition().x, u.path.Peek().getPosition().y + 1)))
            {
                other = map.getTile(new Point(u.path.Peek().getPosition().x, u.path.Peek().getPosition().y + 1));

                if (other != null && u.reachable[other.getPosition().x, other.getPosition().y] >= 0)
                {
                    if (next == null || u.reachable[next.getPosition().x, next.getPosition().y] == -1 || u.reachable[other.getPosition().x, other.getPosition().y] <= u.reachable[next.getPosition().x, next.getPosition().y])
                        next = other;
                }
            }

            // EAST
            if (map.contains(new Point(u.path.Peek().getPosition().x + 1, u.path.Peek().getPosition().y)))
            {
                other = map.getTile(new Point(u.path.Peek().getPosition().x + 1, u.path.Peek().getPosition().y));

                if (other != null && u.reachable[other.getPosition().x, other.getPosition().y] >= 0)
                {
                    if (next == null || u.reachable[next.getPosition().x, next.getPosition().y] == -1 || u.reachable[other.getPosition().x, other.getPosition().y] <= u.reachable[next.getPosition().x, next.getPosition().y])
                        next = other;
                }
            }

            // WEST 
            if (map.contains(new Point(u.path.Peek().getPosition().x - 1, u.path.Peek().getPosition().y)))
            {
                other = map.getTile(new Point(u.path.Peek().getPosition().x - 1, u.path.Peek().getPosition().y));

                if (other != null && u.reachable[other.getPosition().x, other.getPosition().y] >= 0)
                {
                    if (next == null || u.reachable[next.getPosition().x, next.getPosition().y] == -1 || u.reachable[other.getPosition().x, other.getPosition().y] <= u.reachable[next.getPosition().x, next.getPosition().y])
                        next = other;
                }
            }

            if(next == null || next == map.getTile(u.getPosition()))
            {
                return;
            }
            
            u.path.Push(next);
        }
    }
}
