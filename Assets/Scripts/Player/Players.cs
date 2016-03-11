using UnityEngine;
using System.Collections;

public class Players : Player {

  public Players(string x)
  {
    playerName = x;
    MainPhase = new MainPhase(this);
    MovePhase = new MovePhase(this);
    WaitPhase = new WaitPhase(this);
    EndPhase = new EndPhase(this);
    curPhase = WaitPhase;
  }
}
