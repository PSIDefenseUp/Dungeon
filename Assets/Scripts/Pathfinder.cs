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
        u.path = new Queue<Tile>();

        // Add destination as last point in path
        u.path.Enqueue(map.getTile(p));

        // TODO: Until we are back at u's current location, move back from the dest        
    }
}
