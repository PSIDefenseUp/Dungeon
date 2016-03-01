using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour
{
    private Rect bounds;    // The boundaries of the map (top left and bottom right)
    private Tile[,] tiles;  // Array containing all the tiles on the map
    private Unit[,] units;  // Array containing all the units on the map

	// Use this for initialization
	void Start ()
    {
        // Initialize bounds to 0 all around
        bounds = new Rect();

        // Grab the 'object' under which all map tiles are stored
        Transform tileGroup = this.gameObject.transform.FindChild("Tiles");

        // Get boundaries of the map
        for(int i = 0; i < tileGroup.childCount; i++)
        {
            // Grab the tile 'object'
            Vector3 pos = tileGroup.GetChild(i).gameObject.transform.position;

            // Set min and max x-coordinates
            if (pos.x < bounds.x)
                bounds.x = pos.x;
            else if (pos.x > bounds.width)
                bounds.width = pos.x;

            // Set min and max y-coordinates (get pos.z because y is UP in Unity)
            if (pos.z < bounds.y)
                bounds.y = pos.z;
            else if (pos.z > bounds.height)
                bounds.height = pos.z;
        }

        // Initialize arrays to empty
        tiles = new Tile[(int)bounds.width, (int)bounds.height];
        units = new Unit[(int)bounds.width, (int)bounds.height];

        // Insert tiles into array
        for (int i = 0; i < tileGroup.childCount; i++)
        {
            // If the tile doesn't have a tile script attached, there's something crazy going on, skip it to avoid buggy game
            if (tileGroup.GetChild(i).gameObject.GetComponent<Tile>() == null)
                continue;

            // Grab the tile's position
            Vector3 pos = tileGroup.GetChild(i).gameObject.transform.position;

            // Put the tiles in the right place in the tile array
            tiles[(int)pos.x, (int)pos.z] = tileGroup.GetChild(i).gameObject.GetComponent<Tile>();
        }

        // Grab the 'object' under which all units are stored
        Transform unitGroup = this.gameObject.transform.FindChild("Units");
        
        // Insert units into array
        for(int i = 0; i < unitGroup.childCount; i++)
        {
            // If the unit doesn't have a unit script attached, there's something crazy going on, skip it to avoid buggy game
            if (unitGroup.GetChild(i).gameObject.GetComponent<Unit>() == null)
                continue;

            // Grab the unit's position
            Vector3 pos = unitGroup.GetChild(i).gameObject.transform.position;

            // Put the units in the right place in the unit array
            units[(int)pos.x, (int)pos.y] = unitGroup.GetChild(i).gameObject.GetComponent<Unit>();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    // Returns the tile at (p.x, p.y)
    public Tile getTile(Point p)
    {
        return tiles[p.x, p.y];
    }

    // Returns the tile at (x, y)
    public Tile getTile(int x, int y)
    {
        return tiles[x, y];
    }
}
