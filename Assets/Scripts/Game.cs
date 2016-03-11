using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Map map;                 // The map we're playing on

    public List<Player> playerList; // List of players in the game
    public Player currentPlayer;    // simple instance of current player for easy manipulation
    public int currentPlayerIndex;  // Index in the players list of the current player 

    public Cursor cursor;           // The cursor!
    public Camera gameCamera;       // The camera!
    public Canvas uiCanvas;         // UI: Canvas 

    public Text playerTurnText;     // UI: player turn text
    public Dropdown unitSelect;     // UI: dungeon master drop down list

    // Use this for initialization
    void Start()
    {
        // Grab essential game components. This is done through their names as strings, so don't get too creative with editing the scene later.
        map = GameObject.Find("Map").GetComponent<Map>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //playerTurnText = GameObject.Find("playerTurn").GetComponent<Text>();
        //unitSelect = GameObject.Find("dmUnitSelect").GetComponent<Dropdown>();

        playerList = new List<Player>();
        currentPlayerIndex = 0;

        demo();
    }

    // Update is called once per frame
    void Update()
    {
        //ui();
    }


    void demo()
    {
        Player Player1 = new Player(0, "Test Player");
        Player DM = new Player(1, "Dungeon Master");
    }

    public int getPlayerListSize()
    {
        return playerList.Count;
    }

    //UI function: setup to display player turn text 
    private string setupPlayerTurnUI(string playerName)
    {
        return "Player Turn: " + playerName;
    }

    //UI function: manipulate ui elements
    private void ui()
    {

        if (string.Equals(currentPlayer.playerName, "Dungeon Master"))
        {
            unitSelect.enabled = true;
            unitSelect.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            unitSelect.enabled = false;
            unitSelect.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void addPlayer(Player p)
    {
        p.playerIndex = playerList.Count;
        playerList.Add(p);
    }

    public void advanceTurn()
    {
        // Change current player
        currentPlayerIndex++;
        currentPlayerIndex %= playerList.Count;
        currentPlayer = playerList[currentPlayerIndex];
        cursor.setSelectedUnit(null);
        //playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName); TODO: re-enable turn text

        // Give all of this player's units the ability to move and act again
        foreach (Unit u in map.getUnitList())
        {
            if (u.owner == currentPlayer.playerIndex)
            {
                u.canAct = true;
                u.canMove = true;
            }
        }
    }
}
