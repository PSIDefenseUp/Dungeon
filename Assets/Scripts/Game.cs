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
    uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    playerTurnText = GameObject.Find("playerTurn").GetComponent<Text>();
    unitSelect = GameObject.Find("dmUnitSelect").GetComponent<Dropdown>();

    currentPlayerIndex = 0;

    demo();
  }

  // Update is called once per frame
  void Update()
  {
    currentPlayer = playerList[currentPlayerIndex];
    playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName);
    ui();
  }


  void demo()
  {
    Player Player1 = new Player();
    Player DM = new Player();

    Player1.playerName = "Test Player";
    DM.playerName = "Dungeon Master";

    playerList.Add(Player1);
    playerList.Add(DM);

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

}
