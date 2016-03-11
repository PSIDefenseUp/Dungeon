using UnityEngine;
using System.Collections;

public interface IPlayerPhase
{

  void UpdatePhase();

  void ToMainPhase();

  void ToMovePhase();

  void ToEndPhase();

  void ToWaitPhase();

  void nextPhase();

  string toString();

}
