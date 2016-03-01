using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private Point position;     // This tile's position on the map

    public bool solid;          // Is this a solid tile -- one that we cannot path through?
    public int type;            // What type of tile is this? (0 =normal, 1 = wall, 2 = door, etc.)
    public int moveCost = 1;    // The cost of moving through a tile of this type -- Does not apply if the tile is solid

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
