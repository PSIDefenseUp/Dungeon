using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private Game game;          // The game object
    private Point position;     // This tile's position on the map

    public bool solid;          // Is this a solid tile -- one that we cannot path through?
    public int type;            // What type of tile is this? (0 =normal, 1 = wall, 2 = door, etc.)
    public int moveCost = 1;    // The cost of moving through a tile of this type -- Does not apply if the tile is solid

	// Use this for initialization
	void Start ()
  {
        // Grab the current game object from the scene
        game = GameObject.Find("GameManager").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update ()
  {
	    
	}

    void OnMouseEnter()
    {
        // When this tile is moused over, put the cursor on it (we take the cursor off the last tile in this call also)
        game.cursor.selectTile(transform);
    }

    void select()
    {
        
    }

    void deselect()
    {

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
}
