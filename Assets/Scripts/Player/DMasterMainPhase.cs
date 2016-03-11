using UnityEngine;
using System.Collections;

public class DMasterMainPhase : IMainPhase
{
  public readonly Player player;

  public DMasterMainPhase(Player p)
  {
    player = p;
  }


  public void ToEndPhase()
  {
    // Main never goes to End phase 
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
  public void ToBuildPhase()
  {
    // TODO: setup build phase for DM
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
