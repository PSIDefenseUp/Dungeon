using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    private Game game;          // Reference to game object
    private Rect bounds;        // The boundaries of the map (top left and bottom right)
    private Tile[,] tiles;      // Array containing all the tiles on the map
    private Unit[,] units;      // Array containing all the units on the map
    private List<Unit> unitList;   // List of all units on the map

    public Texture collide;
    public Texture noCollide;
    public Texture heroUnitCollide;
    public Texture dmUnitCollide;
    public Texture interactableCollide;

    public bool drawDebugMap = false;

    // Use this for initialization
    void Start()
    {
        // Grab game management object
        game = GameObject.Find("GameManager").GetComponent<Game>();

        // Load Map
        loadMapFromScene();       
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if(drawDebugMap)
        {
            debugDrawCollisionMap();
            debugDrawMoveCosts();
        }
    }

    public void loadMapFromScene()
    {
        // Initialize unit list
        unitList = new List<Unit>();

        // Initialize bounds to 0 all around
        bounds = new Rect();

        // Grab the 'object' under which all map tiles are stored
        Transform tileGroup = this.gameObject.transform.FindChild("Tiles");

        // Get boundaries of the map
        for (int i = 0; i < tileGroup.childCount; i++)
        {
            // Grab the tile 'object'
            Vector3 pos = tileGroup.GetChild(i).gameObject.transform.position;

            // Set min and max x-coordinates
            if (pos.x < bounds.xMin)
                bounds.xMin = pos.x;
            else if (pos.x > bounds.xMax)
                bounds.xMax = pos.x;

            // Set min and max y-coordinates (get pos.z because y is UP in Unity)
            if (pos.z < bounds.yMin)
                bounds.yMin = pos.z;
            else if (pos.z > bounds.yMax)
                bounds.yMax = pos.z;
        }

        bounds.width++;
        bounds.height++;

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
            addTile(Mathf.RoundToInt(pos.x - bounds.xMin), Mathf.RoundToInt(pos.z - bounds.yMin), tileGroup.GetChild(i).gameObject.GetComponent<Tile>());
        }

        // Grab the 'object' under which all units are stored
        Transform unitGroup = this.gameObject.transform.FindChild("Units");

        // Insert units into array
        for (int i = 0; i < unitGroup.childCount; i++)
        {
            // If the unit doesn't have a unit script attached, there's something crazy going on, skip it to avoid buggy game
            if (unitGroup.GetChild(i).gameObject.GetComponent<Unit>() == null)
                continue;

            // Grab the unit's position
            Vector3 pos = unitGroup.GetChild(i).gameObject.transform.position;

            // Put the units in the right place in the unit array
            addUnit(Mathf.RoundToInt(pos.x - bounds.xMin), Mathf.RoundToInt(pos.z - bounds.yMin), unitGroup.GetChild(i).gameObject.GetComponent<Unit>());
        }
    }

    public void addTile(Point p, Tile t)
    {
        addTile(p.x, p.y, t);
    }

    public void addTile(int x, int y, Tile t)
    {
        t.setPosition(x, y);
        tiles[x, y] = t;
    }

    public void addUnit(Point p, Unit u)
    {
        addUnit(p.x, p.y, u);
    }

    public void addUnit(int x, int y, Unit u)
    {
        u.setPosition(x, y);
        units[x, y] = u;
        unitList.Add(u);
    }

    public void removeUnit(Unit u)
    {
        unitList.Remove(u);
        units[u.getPosition().x, u.getPosition().y] = null;
        Destroy(u.gameObject);
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

    // Returns the unit at (p.x, p.y)
    public Unit getUnit(Point p)
    {
        return units[p.x, p.y];
    }

    // Returns the unit at (x, y)
    public Unit getUnit(int x, int y)
    {
        return units[x, y];
    }

    public void moveUnit(Point start, Point dest)
    {
        if (!contains(dest) && contains(start))
            return;

        // Don't move anywhere if the unit isn't trying to go anywhere
        if (!(start.x == dest.x && start.y == dest.y))
        {
            // Move the unit
            units[dest.x, dest.y] = units[start.x, start.y];
            units[start.x, start.y] = null;

            // set unit's movement path
            units[dest.x, dest.y].pathfinder.getPath(units[dest.x, dest.y], dest);
        }

        // update unit's position, take away its ability to move for the turn, and stop highlighting tiles it could have moved to
        units[dest.x, dest.y].setPosition(dest);
        units[dest.x, dest.y].removeHighlights();
        units[dest.x, dest.y].canMove = false;
    }

    public Rect getBounds()
    {
        return this.bounds;
    }

    public List<Unit> getUnitList()
    {
        return unitList;
    }

    public bool contains(Point p)
    {
        return p.x >= 0 && p.x < bounds.width && p.y >= 0 && p.y < bounds.height;
    }

    public void debugDrawCollisionMap()
    {
        const int tileSize = 16;

        for (int y = 0; y < bounds.height; y++)
        {
            for (int x = 0; x < bounds.width; x++)
            {
                // Draw minimap
                if (tiles[x, y] != null && !tiles[x, y].solid)
                {
                    if (units[x, y] != null)
                    {
                        if (units[x, y] is Interactable)
                        {
                            GUI.DrawTexture(new Rect(x * tileSize, (bounds.height - y) * tileSize, tileSize, tileSize), interactableCollide);
                        }
                        else
                        {
                            if (units[x, y].team == 0)
                                GUI.DrawTexture(new Rect(x * tileSize, (bounds.height - y) * tileSize, tileSize, tileSize), heroUnitCollide);
                            else
                                GUI.DrawTexture(new Rect(x * tileSize, (bounds.height - y) * tileSize, tileSize, tileSize), dmUnitCollide);
                        }
                    }
                    else
                    {
                        GUI.DrawTexture(new Rect(x * tileSize, (bounds.height - y) * tileSize, tileSize, tileSize), noCollide);
                    }
                }
                else
                {
                    GUI.DrawTexture(new Rect(x * tileSize, (bounds.height - y) * tileSize, tileSize, tileSize), collide);
                }
            }
        }
    }

    public void debugDrawMoveCosts()
    {

    }
}
