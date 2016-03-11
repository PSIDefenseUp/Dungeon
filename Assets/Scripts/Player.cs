using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    public string playerName;     // Player Name
    public int playerIndex;       // Which player number is this?
    public int team;              // The 'team' this player is on -- 0: heroes, 1: dungeon master

    //public List<Unit> units;      // List of Units on the board the player controls
    //public bool isTurn = false; // boolean for later use of highlighting player units during their turn

    public Player(int team, string name)
    {
        this.playerName = name;
        this.team = team;
        GameObject.Find("GameManager").GetComponent<Game>().addPlayer(this); // Adds the player to the game's player array and gives us its index     
    }
}