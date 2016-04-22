using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game : MonoBehaviour
{
    public Map map;                 // The map we're playing on
      
    public List<Player> playerList; // List of players in the game
    public Player currentPlayer;    // simple instance of current player for easy manipulation
    public int currentPlayerIndex;  // Index in the players list of the current player 
    public bool building;           // Are we in the DM's building phase?

    public Cursor cursor;           // The cursor!
    public Camera gameCamera;       // The camera!
    public Canvas uiCanvas;         // UI: Canvas 

    public Text playerTurnText;     // UI: player turn text
    public Button End;              // UI: End Button

    public bool gameOver = false;   // Is the game over?

    public LoadScreen loadScreen;   // Load screen, used to load other scenes

    // Use this for initialization
    void Start()
    {
        // Grab essential game components. This is done through their names as strings, so don't get too creative with editing the scene later.
        map = GameObject.Find("Map").GetComponent<Map>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        uiCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        playerTurnText = GameObject.Find("playerTurn").GetComponent<Text>();
        End = GameObject.Find("EndButton").GetComponent<Button>();
        loadScreen = GameObject.Find("LoadScreen").GetComponent<LoadScreen>();

        // After grabbing the load screen, turn it off until we need it.
        loadScreen.gameObject.SetActive(false);        

        // Perform game setup
        setup();              
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            if(Input.anyKeyDown)
            {
                GameObject.Find("GUI").gameObject.SetActive(false);
                loadScreen.gameObject.SetActive(true);
                loadScreen.loadScene("MainMenu");
            }

            return;
        }

        uiViewables();
        checkGameEnd();

         // When space is pressed, go to the next turn (for testing purposes) -- TODO: DELETE THIS
        if (Input.GetKeyDown(KeyCode.Space))
        {
            advanceTurn();
        }
    }

    void setup()
    {
        // Initialize players
        playerList = new List<Player>();
        Player Player1 = new Player(0, "Test Player");
        Player DM = new Player(1, "Dungeon Master");

        // Set starting player as DM and initialize DM build phase
        currentPlayerIndex = 1;
        currentPlayer = playerList[currentPlayerIndex];
        building = true;
    }

    public int getPlayerListSize()
    {
        return playerList.Count;
    }

    // UI function: setup to display player turn text 
    private string setupPlayerTurnUI(string playerName)
    {
        return "Player Turn: " + playerName;
    }

    // UI function: manipulate ui elements
    private void uiViewables()
    {
        End.enabled = true;
        playerTurnText.text = setupPlayerTurnUI(currentPlayer.playerName);
    }
    
    public void addPlayer(Player p)
    {
        p.playerIndex = playerList.Count;
        playerList.Add(p);
    }

    public void advanceTurn()
    {
        // If we were in the DM build phase, end it forever
        if (building)
        {
            building = false;
            GameObject.Find("DMGold").GetComponent<Text>().gameObject.SetActive(false);
        }

        // Change current player
        currentPlayerIndex++;
        currentPlayerIndex %= playerList.Count;
        currentPlayer = playerList[currentPlayerIndex];
        cursor.selectUnit(null);

        // Give all of this player's units the ability to move and act again
        foreach (Unit u in map.getUnitList())
        {
            if (u.owner == currentPlayer.playerIndex)
            {
                u.refresh();
            }
            else
            {
                u.endTurn();
            }
        }
    }

    public void checkGameEnd()
    {
        // Count number of units owned by each team. If either team has no more units, the other team wins!

        int heroCount = 0;
        int dmCount = 0;

        foreach(Unit u in map.getUnitList())
        {
            if (u.team == 0)
                heroCount++;
            else if (u.team == 1)
                dmCount++;
        }

        if (dmCount == 0)
        {
            // HEROES WIN!
           // GameObject.Find("GUI").GetComponent<GameEnd>().endGame(0);
        }
        else if (heroCount == 0)
        {
            // DM WINS!
           // GameObject.Find("GUI").GetComponent<GameEnd>().endGame(1);
        }
    }
}
