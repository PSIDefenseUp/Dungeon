using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Game : MonoBehaviour
{
  public Map map;                 // The map we're playing on

  [SerializeField]
  public List<Player> playerList; // List of players in the game

  [SerializeField]
  public Player currentPlayer;    // simple instance of current player for easy manipulation
  public int currentPlayerIndex;  // Index in the players list of the current player 
  public string currentPhase;
  bool inGame;

  public Cursor cursor;           // The cursor!
  public Camera gameCamera;       // The camera!
  public Canvas uiCanvas;         // UI: Canvas 

  public Text playerTurnText;     // UI: player turn text
  public Text playerPhase;        // UI: player Phase text
  public Dropdown unitSelect;     // UI: dungeon master drop down list
  public Toggle unitSelectToggle; // UI: duingeon master toggle 
  public Button next;             // UI: Next button
  public Button End;              // UI: End Button

  // Use this for initialization
  void Start()
  {

    // Grab essential game components. This is done through their names as strings, so don't get too creative with editing the scene later.
    map = GameObject.Find("Map").GetComponent<Map>();
    cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
    gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    playerTurnText = GameObject.Find("playerTurn").GetComponent<Text>();
    playerPhase = GameObject.Find("playerPhase").GetComponent<Text>();
    unitSelect = GameObject.Find("unitSelectDropdownList").GetComponent<Dropdown>();
    unitSelectToggle = GameObject.Find("placeUnitToggle").GetComponent<Toggle>();
    next = GameObject.Find("NextButton").GetComponent<Button>();
    End = GameObject.Find("EndButton").GetComponent<Button>();

    playerList = new List<Player>();

    demoPlayers();

    currentPlayerIndex = 0;
    inGame = true;

    currentPlayer = playerList[currentPlayerIndex];
    currentPhase = currentPlayer.curPhase.toString();

    StartCoroutine(runGame());
  }

  // Update is called once per frame
  void Update()
  {
    currentPlayer = playerList[currentPlayerIndex];
    currentPhase = currentPlayer.curPhase.toString();
    playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName);
    playerPhase.text = setupPlayerPhaseUI(currentPlayer.curPhase.toString());
    uiViewables();
    runStateMachines();
  }

  IEnumerator runGame()
  {
    while (inGame)
    {
      //TODO: implement a better loop for game for now UI driven loop works
      yield return null;
    }
      //TODO: Setup end game 
  }

  // created players for Demo purposes
  void demoPlayers()
  {
    Players Player1 = new Players("Test Player");
    DungeonMaster DM = new DungeonMaster("Dungeon Master");

    playerList.Add(Player1);
    playerList.Add(DM);
  }

  // return the number of player in game
  public int getPlayerListSize()
  {

    return playerList.Count;

  } 

  public void nextPlayer()
  {
    currentPlayer.isTurn = false;
    currentPlayerIndex++;
    if ( currentPlayerIndex >= playerList.Count)
    {
      currentPlayerIndex = 0;
    }
  }

  //UI function: setup to display player turn text 
  private string setupPlayerTurnUI(string playerName)
  {
    return "Player Turn: " + playerName;
  }

  //UI function: setup to display player turn text 
  private string setupPlayerPhaseUI(string phase)
  {
    return "Phase: " + phase;
  }

  //UI function: manipulate ui elements
  private void uiViewables()
  {
    if (string.Equals(currentPlayer.playerName, "Dungeon Master"))
    {
      unitSelect.enabled = true;
      unitSelect.transform.localScale = new Vector3(1, 1, 1);
      unitSelectToggle.transform.localScale = new Vector3(1, 1, 1);
    }
    else
    {
      unitSelect.enabled = false;
      unitSelect.transform.localScale = new Vector3(0, 0, 0);
      unitSelectToggle.transform.localScale = new Vector3(0, 0, 0);
    }
      if (!string.Equals(currentPhase, "End"))
      {
        End.enabled = false;
        next.enabled = true;
        next.transform.localScale = new Vector3(1, 1, 1);
      }
      else
      {
        End.enabled = true;
        next.enabled = false;
        next.transform.localScale = new Vector3(0, 0, 0);
      }
    }

  private void runStateMachines()
{
  foreach (Player x in playerList)
  {
    x.Update();
  }
}
    /*
    public void advanceTurn()
    {
        // Change current player
        currentPlayerIndex++;
        currentPlayerIndex %= playerList.Count;
        currentPlayer = playerList[currentPlayerIndex];
        cursor.setSelectedUnit(null);
        //playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName); TODO: re-enable turn text

        // Give all of this player's units the ability to move and act again
        foreach(Unit u in map.getUnitList())
        {
            if (u.owner == currentPlayer.playerIndex)
            {
                u.canAct = true;
                u.canMove = true;
            }
        }
    }
*/
}
