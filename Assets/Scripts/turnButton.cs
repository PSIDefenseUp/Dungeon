using UnityEngine;
using System.Collections;

public class turnButton : MonoBehaviour
{ 
  public Game managerRef;

  // Use this for initialization
  void Start ()
  {
    managerRef = GameObject.Find("GameManager").GetComponent<Game>();
  }
  void Update()
  {
  }

  public void nextPlayerTurn()
  {
    managerRef.currentPlayer.curPhase = managerRef.currentPlayer.WaitPhase;
    managerRef.currentPlayer.isTurn = false;
    managerRef.nextPlayer();
  }
	
/*
    // Use this for initialization
    void Start ()
    {
        managerRef = GameObject.Find("GameManager").GetComponent<Game>();
    }

    void Update()
    {
    }

    public void nextPlayerTurn()
    {
        managerRef.advanceTurn();
    }	
 */

}
