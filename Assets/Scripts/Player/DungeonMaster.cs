using UnityEngine;
using System.Collections;

public class DungeonMaster : Player {
  // TODO: set up build phase

  public DungeonMaster(string x)
  {
    playerName = x;
    MainPhase = new DMasterMainPhase(this);
    MovePhase = new MovePhase(this);
    WaitPhase = new WaitPhase(this);
    EndPhase = new EndPhase(this);
    curPhase = WaitPhase;
  }
}
