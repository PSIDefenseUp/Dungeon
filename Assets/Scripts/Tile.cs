using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    private Point position; // This tile's position on the map

    public bool solid;      // Is this a solid tile -- one that we cannot path through?
    public int type;        // What type of tile is this? (0 = normal, 1 = wall, 2 = door, etc.)

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
