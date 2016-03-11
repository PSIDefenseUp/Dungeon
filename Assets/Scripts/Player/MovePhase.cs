using UnityEngine;
using System.Collections;
using System;

public class MovePhase : IPlayerPhase
{
  public readonly Player player;

  public MovePhase(Player p)
  {
    player = p;
  }

  public void ToEndPhase()
  {
    player.curPhase = player.EndPhase;
  }

  public void ToMainPhase()
  {
    player.curPhase = player.MainPhase;
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
    //TODO: setup highlight units that can move during this players turn
  }

  // Default Next Phase if you doesnt define next pohase for turn
  public void nextPhase()
  {
    player.curPhase = player.EndPhase;
  }

  public string toString()
  {
    return "Move";
  }
}
