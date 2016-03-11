using UnityEngine;
using System.Collections;

public class EndPhase : IPlayerPhase {

  public readonly Player player;

  public EndPhase(Player p)
  {
    player = p;
  }

  public void ToEndPhase()
  {
    // End doesnt go to end phase
  }

  public void ToMainPhase()
  {
    // End doesnt go to end phase
  }

  public void ToMovePhase()
  {
    // Move never goes to move phase
  }

  public void ToWaitPhase()
  {
    player.curPhase = player.WaitPhase;
  }

  public void UpdatePhase()
  {
    //TODO: setup time to end turn
    nextPhase();
  }

  // Default Next Phase if you doesnt define next pohase for turn
  public void nextPhase()
  {
    player.curPhase = player.WaitPhase;
  }
  public string toString()
  {
    return "End";
  }
}
