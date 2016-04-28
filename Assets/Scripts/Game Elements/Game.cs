using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Game : NetworkBehaviour
{
  [System.Serializable]
  public struct player
  {
    public string playerName;     // Player Name
    public int playerIndex;       // Which player number is this?
    public int team;              // The 'team' this player is on -- 0: heroes, 1: dungeon master
    public int keyCount;          // The number of keys this player possesses

    public player(int x, string name)
    {
      playerName = name;
      playerIndex = 0;
      team = x;
      keyCount = 0;
    }
  }

  [System.Serializable]
  public class SyncListPlayer : SyncListStruct<player>
  {

  }
  

    //Client Variables
    public Map map;                 // The map we're playing on 
    public Camera gameCamera;
    public Cursor cursor;
    public LoadScreen loadScreen;   // Load screen, used to load other scenes 


    //Server Variables   
    [SyncVar]
    public player currentPlayer;    // simple instance of current player for easy manipulation
    [SyncVar]
    public int currentPlayerIndex;  // Index in the players list of the current player 
    [SyncVar]
    public bool gameOver;           // Is the game over?
    [SyncVar]
    public bool building;           // Is Building
    [SerializeField]
    public SyncListPlayer playerList = new SyncListPlayer(); // List of players in the game
    [SyncVar]
    public bool gameStart;
    [SyncVar]
    public bool HUD;


    // Use this for initialization
    void Start()
    {
        // Grab essential game components. This is done through their names as strings, so don't get too creative with editing the scene later.
        gameStart = false;
        gameOver = false;
        HUD = false;
        map.gameObject.SetActive(true);
 
        // After grabbing the load screen, turn it off until we need it.
        //loadScreen = GameObject.Find("LoadScreen").GetComponent<LoadScreen>();
        //loadScreen.gameObject.SetActive(false);        
           
    }

    // Update is called once per frame
    void Update()
    {
          if (!gameStart)
          {
            if(playerList.Count == 2)
            {
                gameStart = true;
                HUD = true;
                Setup();
            }
          }
            
         /*    if (gameOver)
         {
            if(Input.anyKeyDown)
            {
                Gui.SetActive(false);
                loadScreen.gameObject.SetActive(true);
                loadScreen.loadScene("MainMenu");
            }

            return;
         }
         */
  
       // checkGameEnd();
        
    }
 
    void Setup()
    {

        // Set starting player as DM and initialize DM build phase
        for( int i = 0; i < 2;i++)
        {
            if (playerList[i].team == 1) { currentPlayerIndex = i; break; }
            else{ currentPlayerIndex = -1; } 
        }
        
        if(currentPlayerIndex < 0) { return; }

        currentPlayer = playerList[currentPlayerIndex];
        building = true;
    }


    public void advanceTurn()
    {
        // If we were in the DM build phase, end it forever
        if (building)
        {
            building = false;
            GameObject.Find("DMGold").GetComponent<Text>().gameObject.SetActive(false);
            RpcClearDMGoldText();
            RpcTellClientToAdvance();
            return;
        }

        // Change current player
        currentPlayerIndex++;
        currentPlayerIndex %= playerList.Count;
        currentPlayer = playerList[currentPlayerIndex];
        RpcTellClientToAdvance();
    }

    public void checkGameEnd()
    {
        // Don't check for game end in building phase
        if (building)
            return;

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

  //-------------------------------------------------------------------------------------
  //Server
  //-------------------------------------------------------------------------------------
  [ClientRpc]
  public void RpcTellClientToAdvance()
  {
    cursor.selectUnit(null);
    clientAdvance();
  }
  [ClientRpc]
  public void RpcClearDMGoldText()
  {
    //GameObject.Find("DMGold").GetComponent<Text>().gameObject.SetActive(false);
  }

  //-------------------------------------------------------------------------------------
  //Client
  //-------------------------------------------------------------------------------------
  [Client]
  public void clientAdvance()
  {
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
  [Client]
  public void addPlayer(player p)
  {
    if ( playerList.Count > 1) { return; }
    
    p.playerIndex = playerList.Count;
    playerList.Add(p);
  }
}
