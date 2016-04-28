using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class UI : NetworkBehaviour {

  public Canvas uiCanvas;         // UI: Canvas
  public Button End;              // UI: End Button
  public Text playerTurnText;     // UI: player turn text
  public Cursor cursor;
  public Game game;
  public networkPlayerScript netPlayer;

  public GameObject pttObj;
  public GameObject eObj;
  public GameObject gObj;

  public GameObject Gui;          
  public GameObject GameHud;      

  // Use this for initialization
  void Start ()
  {
    playerTurnText = GameObject.Find("playerTurn").GetComponent<Text>(); 
    End = GameObject.Find("EndButton").GetComponent<Button>();
    GameHud = GameObject.Find("GameHud");
    Gui = GameObject.Find("GUI");
    cursor = this.gameObject.GetComponent<Cursor>();

    GameHud.SetActive(false);
    Gui.SetActive(false);

    gObj = GameObject.Find("GameManager");
    if (gObj != null) { game = gObj.GetComponent<Game>(); }


  }
	
	// Update is called once per frame
	void Update ()
  {
    if (!isLocalPlayer) { return; }

      checkVars();

	    if( game != null && game.gameStart )
      { 
          uiViewables();
      }
	}

  void checkVars()
  {
    if (gObj == null)
    {
      gObj = GameObject.Find("GameManager");
      if (gObj != null)
        game = gObj.GetComponent<Game>();
    }
  }

  // UI function: setup to display player turn text 
  private string setupPlayerTurnUI(string playerName)
  {
    return "Player Turn: " + playerName;
  }

  // UI function: manipulate ui elements
  private void uiViewables()
  {
    if (game.HUD)
    {
      GameHud.SetActive(true);
      Gui.SetActive(true);
    }
    else
    {
      Gui.SetActive(false);
      GameHud.SetActive(false);
    }

    playerTurnText.text = setupPlayerTurnUI(game.currentPlayer.playerName);

    if (game.currentPlayer.team == netPlayer.myPlayerInfo.team){ End.gameObject.SetActive(true); }
    else { End.gameObject.SetActive(false); }
  }

}
