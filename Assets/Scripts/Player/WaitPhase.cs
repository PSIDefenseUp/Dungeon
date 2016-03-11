using UnityEngine;
using System.Collections;
using System;

public class WaitPhase : IPlayerPhase
{
  public readonly Player player;

  public WaitPhase(Player p)
  {
    player = p;
  }
  public void ToEndPhase()
  {
    // Wait never goes to End phase
  }

  public void ToMainPhase()
  {
    player.curPhase = player.MainPhase;
  }

  public void ToMovePhase()
  {
    player.curPhase = player.MovePhase;
  }

  public void ToWaitPhase()
  {
    // Wait never goes to wait phase
  }

  public void UpdatePhase()
  {
    // TODO: define anything the player can do as they wait for other player to take there turn
  }

  // Default Next Phase if user doesnt define next phase for turn
  public void nextPhase()
  {
    player.curPhase = player.MainPhase;
  }

  public string toString()
  {
    return "Wait";
  }
}
