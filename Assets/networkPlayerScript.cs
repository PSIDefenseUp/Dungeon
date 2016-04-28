using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

public class networkPlayerScript : NetworkBehaviour {

  [SyncVar]
  [Header("Player Info")]
  [SerializeField]
  public string myName;

  [SyncVar]
  [SerializeField]
  public Game.player myPlayerInfo;

  [SerializeField]
  public Cursor cursor;

  [Space(10)]
  [Header("List of Locs/Units to Spawn")]
  [SerializeField]
  public List<GameObject> HeroUnitsToSpawn;
  public List<GameObject> HeroSpawnLocations;
  public List<GameObject> DMUnitsToSpawn;
  public List<GameObject> DMSpawnLocations;

  public SyncListString spawnedUnits = new SyncListString();

  private bool isAdded = false;
  private GameObject GM;
  private GameObject SL;

  public Game game;         
  public Light spotL;


  void Start()
  {
    // Grab Game object from scene
    GM = GameObject.Find("GameManager");
    if (GM != null)
      game = GM.GetComponent<Game>();
    
    // Grab Spotlight from scene 
    SL = GameObject.Find("Spotlight");
    if (isLocalPlayer && SL != null)
      spotL = SL.GetComponent<Light>();

  }

  // Update is called once per frame
  void Update()
  {

    if (this.name == "" || this.name == "NetworkPlayer(Clone)")
      setName();

    if (!isLocalPlayer)
      return;

    if (GM == null )
    {
      GM = GameObject.Find("GameManager");
      if (GM != null)
        game = GM.GetComponent<Game>();
    }

    if (SL == null)
    {
      SL = GameObject.Find("Spotlight");
      if ( SL != null)
        spotL = SL.GetComponent<Light>();
    }

    if (!isAdded && game != null && myPlayerInfo.playerName != "")
    {
      CmdTellServerToAddPlayer();
      game.cursor = cursor;
      isAdded = true;
    }
  }

  //-------------------------------------------------------------------------------------
  //Init - code that runs on start  
  //-------------------------------------------------------------------------------------

  public override void OnStartLocalPlayer()
  {
    base.OnStartLocalPlayer();
    CmdTellServerMyName(getName());
    CmdTellServerMyPlayer(getPlayer());
    setName();
  }

  

  //-------------------------------------------------------------------------------------
  //Everyone - code that runs on client and server alike
  //-------------------------------------------------------------------------------------
  void setName()
  {
    if (!isLocalPlayer)
    {
      this.name = myName;
    }
    else
    {
      this.name = getName();
    }
  }

  private void AddUnitsToMap()
  {
    GameObject o = GameObject.Find("Units");
    foreach ( var name in spawnedUnits)
    {
      GameObject x = GameObject.Find(name);
      
        x.transform.parent = o.transform;

      Vector3 pos = x.transform.position;
      Rect bounds = game.map.getBounds();

      game.map.addUnit(Mathf.RoundToInt(pos.x - bounds.xMin), Mathf.RoundToInt(pos.z - bounds.yMin), x.GetComponent<Unit>());
    }
    spawnedUnits.Clear();
  }
  //-------------------------------------------------------------------------------------
  //Client - code that runs on the client
  //-------------------------------------------------------------------------------------
  [Client]
  private void HeroSceneUnits()
  {
    for(int i = 0; i < HeroSpawnLocations.Count; i++ )
    {
      if (HeroSpawnLocations[i])
      {
        Vector3 spawnPosition = new Vector3(HeroSpawnLocations[i].transform.position.x, HeroSpawnLocations[i].transform.position.y, HeroSpawnLocations[i].transform.position.z);
        Quaternion SpawnRotation = Quaternion.Euler(0, 180, 0);
        if (HeroUnitsToSpawn[i])
        {
          GameObject Unit = (GameObject)Instantiate(HeroUnitsToSpawn[i], spawnPosition, SpawnRotation);
          NetworkServer.Spawn(Unit);
          spawnedUnits.Add(Unit.name);
        }
       }
      }
    }
  [Client]
  private void DMSceneUnits()
  {
    for (int i = 0; i < DMSpawnLocations.Count; i++)
    {
      if (DMSpawnLocations[i])
      {
        Vector3 spawnPosition = new Vector3(DMSpawnLocations[i].transform.position.x, DMSpawnLocations[i].transform.position.y, DMSpawnLocations[i].transform.position.z);
        Quaternion SpawnRotation = Quaternion.Euler(0, 180, 0);
        if (DMUnitsToSpawn[i])
        {
          GameObject Unit = (GameObject)Instantiate(DMUnitsToSpawn[i], spawnPosition, SpawnRotation);
          NetworkServer.Spawn(Unit);
          spawnedUnits.Add(Unit.name);
        }
       }
     }
   }
  [Client]
  string getName()
  {
    string x = GameObject.Find("PlayerName").GetComponent<InitGameSequence>().myName;
    return x;
  }

  [Client]
  Game.player getPlayer()
  {
    Game.player x = GameObject.Find("PlayerName").GetComponent<InitGameSequence>().myPlayer;
    return x;
  }

  //-------------------------------------------------------------------------------------
  //Command - code that run on client and force the server to update all clients on server
  //-------------------------------------------------------------------------------------
  [Command]
  void CmdTellServerMyPlayer(Game.player p)
  {
    myPlayerInfo = p;
  }

  [Command]
  void CmdTellServerMyName(string x)
  {
    myName = x;
  }

  [Command]
  void CmdTellServerToAddPlayer()
  {
    foreach (var x in game.playerList)
    {
      if (x.team == 0 && myPlayerInfo.team == 0)
      {
        Game.player dupe = new Game.player(1, myName + " - DM");
        dupe.playerIndex = game.playerList.Count;
        myPlayerInfo = dupe;
        break;
      }

      if (x.team == 1 && myPlayerInfo.team == 1)
      {
        Game.player dupe = new Game.player(0, myName + " - Hero");
        dupe.playerIndex = game.playerList.Count;
        myPlayerInfo = dupe;
        break;
      }
    }

    //add me to game server for sync
    game.addPlayer(myPlayerInfo);

   /*
    if (myPlayerInfo.team == 0)
      HeroSceneUnits();
    else
      DMSceneUnits();
   */
  }

  [Command]
  void CmdAdvanceTurn()
  {
    game.advanceTurn();
  }

  [Command]
  void CmdChangeCursorName(GameObject c, string name)
  {
    c.name = name + " - Cursor";
  }
  //-------------------------------------------------------------------------------------
  //Server - code that runs on server 
  //-------------------------------------------------------------------------------------




  //-------------------------------------------------------------------------------------
  //Hook - functions called from server when syncvar change 
  //-------------------------------------------------------------------------------------
}

