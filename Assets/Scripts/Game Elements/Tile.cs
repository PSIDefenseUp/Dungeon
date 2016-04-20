using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private Game game;          // The game object
    public Point position;     // This tile's position on the map

    public bool solid;          // Is this a solid tile -- one that we cannot path through?
    public bool occupy;         // is this tile occupied
    public int type;            // What type of tile is this? (0 =normal, 1 = wall, 2 = door, etc.)
    public int moveCost = 1;    // The cost of moving through a tile of this type -- Does not apply if the tile is solid

    public Tile parent;
    public int f;
    public int g;
    public int h;

    // Use this for initialization
    void Start()
    {
        // Grab the current game object from the scene
        game = GameObject.Find("GameManager").GetComponent<Game>();
        occupy = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        // When this tile is moused over, put the cursor on it (we take the cursor off the last tile in this call also)
        game.cursor.setCurrentTile(transform);
    }

    // Highlights this tile in the color provided
    public void highlight(Color c)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", c);
        }
    }

    public Point getPosition()
    {
        return this.position;
    }

    public void setPosition(Point p)
    {
        this.position = p;
    }

    public void setPosition(int x, int y)
    {
        this.position = new Point(x, y);
    }

    // return directon from point to this tile 1 = north, 2 = east, 3 = south, 4 = west, 0 = error 
    public int getDirection(Point fromPoint)
    {

    Debug.Log("FROM x: " + fromPoint.x +"y: "+fromPoint.y+"TO x: "+position.x+"y: " + position.y);

    if (position.y > fromPoint.y)
    {
      Debug.Log("Tile is South");
      return 3;
    }
    else if (position.x > fromPoint.x)
    {
      Debug.Log("Tile is East");
      return 2;
    }
    else if (position.y < fromPoint.y)
    {
      Debug.Log("Tile is North");
      return 1;
    }
    else if (position.x < fromPoint.x)
    {
      Debug.Log("Tile is West");
      return 4;
    }
    else
    {
      Debug.Log("Error in tile Compare");
      return 0;
    }

    }

    public bool isWalkable(Tile x)
    {
      return x.type == 0;
    }
}
