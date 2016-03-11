using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class Game : MonoBehaviour
{
    public Map map;                 // The map we're playing on
    public Unit Warrior;            // cloneable warrior
  
  //these need rework. but in efforts of a good presentation they work 
    public GameObject unitsObj;     //
    public Unit buildObj;           //
    public List<Unit> attackList;   //
    public Toggle attackT;          //  

  public List<Player> playerList; // List of players in the game
    public Player currentPlayer;    // simple instance of current player for easy manipulation
    public int currentPlayerIndex;  // Index in the players list of the current player 

    public Cursor cursor;           // The cursor!
    public Camera gameCamera;       // The camera!
    public Canvas uiCanvas;         // UI: Canvas 

    public Text playerTurnText;     // UI: player turn text
    public Dropdown unitSelect;     // UI: dungeon master drop down list
    public Toggle unitSelectToggle; // UI: duingeon master toggle 
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
        unitSelect = GameObject.Find("unitSelectDropdownList").GetComponent<Dropdown>();
        unitSelectToggle = GameObject.Find("placeUnitToggle").GetComponent<Toggle>();
        End = GameObject.Find("EndButton").GetComponent<Button>();

        // Needs revamp
        Warrior = GameObject.Find("unitForClone").GetComponent<Unit>();
        unitsObj = GameObject.Find("Units").GetComponent<GameObject>();
        attackT = GameObject.Find("AttackToggle").GetComponent<Toggle>();

        playerList = new List<Player>();
        attackList = new List<Unit>();
        unitSelectToggle.enabled = false;

        demo();

        currentPlayer = playerList[currentPlayerIndex];
        currentPlayerIndex = 0;

       
    }

    // Update is called once per frame
    void Update()
    {
       uiViewables();
       buildObjSet();
       setupAttack();
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
  private void uiViewables()
  {
    // simple hide/show hack for the build phase of DM
    if (string.Equals(currentPlayer.playerName, "Dungeon Master"))
    {
      unitSelect.enabled = true;
      unitSelect.transform.localScale = new Vector3(1, 1, 1);
      unitSelectToggle.enabled = true;
      unitSelectToggle.transform.localScale = new Vector3(1, 1, 1);
    }
    else
    {
      unitSelect.enabled = false;
      unitSelect.transform.localScale = new Vector3(0, 0, 0);
      unitSelectToggle.enabled = false;
      unitSelectToggle.transform.localScale = new Vector3(0, 0, 0);
    }
    // hide cursor if build phase
    /*
    if (unitSelectToggle.IsActive() && unitSelectToggle.isOn)
    {
      cursor.transform.localScale = new Vector3(0, 0, 0);
    }
    else
    {
      cursor.transform.localScale = new Vector3(1, 1, 1);
    }
    */

    End.enabled = true;
    playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName);
  }


  // TODO: Needs revamp but it works
  // Duck taped build function while you are dungeonmaster 
  // and build is selected press N to make a unit at cursor position
  public void buildObjSet()
  {

    if(!unitSelectToggle.isOn || !unitSelectToggle.IsActive())
    {
      buildObj = null;
      return;
    }

    if (Input.GetKeyDown(KeyCode.N) && unitSelect.value == 0)
    {
      buildObj = Instantiate(Warrior, cursor.transform.position, Warrior.transform.rotation) as Unit;
      map.addUnit(cursor.getPosition(), buildObj);
      buildObj.transform.parent = unitsObj.transform;
      buildObj = null;
    }
  }
  //TODO: Needs revamp but it works
  // press B to add current mouse over unit the hit attack to attack first unit specified 
  // press V to attack
  public void setupAttack()
  {
    if (attackT.isOn != true)
    {
      attackList.Clear();
      return;
    }

    Unit x = map.getUnit(cursor.getPosition());

    if (Input.GetKeyDown(KeyCode.B))
    {
      if (attackList.Count >= 1 && x != null)
      {
        attackList.Add(x);
      }
      else if (x.owner == currentPlayerIndex && x != null)
      {
        attackList.Add(x);
      }
      else
      {
        //do nothing
      }
    }
    if (Input.GetKeyDown(KeyCode.V))
    {
      AttackUnit();
    }
  }
  private void AttackUnit()
  {
    List<Unit> x = attackList;

    if (x.Count >= 2)
    {
      x[0].Attack(x[1]);
      x = null;
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
