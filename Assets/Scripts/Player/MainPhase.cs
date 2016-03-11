using UnityEngine;
using System.Collections;

public class MainPhase : IMainPhase {

  public readonly Player player;

  public MainPhase(Player p)
  {
    player = p;
  }


  public void ToEndPhase()
  {
    player.curPhase = player.EndPhase;
  }

  public void ToMainPhase()
  {
    // Main never goes to Main phase
  }

  public void ToMovePhase()
  {
    player.curPhase = player.MovePhase;
  }

  public void ToWaitPhase()
  {
    // Main never goes to Wait phase
  }

  public void UpdatePhase()
  {
    // TODO: highlight players that can take actions to allow player manipulation
  }

  // Default Next Phase if you doesnt define next pohase for turn
  public void nextPhase()
  {
    player.curPhase = player.MovePhase;
  }

  public string toString()
  {
    return "Main";
  }
}
