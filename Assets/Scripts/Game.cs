using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public Map map;                 // The map we're playing on

    public List<Player> players;    // List of players in the game
    public int currentPlayer = 0;   // Index in the players list of the current player (TODO: IMPLEMENT TURN SYSTEM & PLAYER OBJECT)

    public Cursor cursor;           // The cursor!
    public Camera gameCamera;       // The camera!

    // Use this for initialization
    void Start ()
    {
        // Grab essential game components. This is done through their names as strings, so don't get too creative with editing the scene later.
        map = GameObject.Find("Map").GetComponent<Map>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
