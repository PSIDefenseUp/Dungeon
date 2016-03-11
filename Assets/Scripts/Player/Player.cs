using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player  {

  public string playerName;   // Player Name
  public List<Unit> units;    // List of Units on the board the player controls
  public bool isTurn = false; // boolean for later use of highlighting player units during their turn

  public IPlayerPhase curPhase;     // player current phase
  public MovePhase MovePhase;       // player move phase ex. move units
  public WaitPhase WaitPhase;       // player wait phase ex. player wait for other players to take their turn
  public IMainPhase MainPhase;
  public EndPhase EndPhase;

  public void Update()
  {
    if (isTurn)
    {
      curPhase.UpdatePhase();
    }
  }

}
