using UnityEngine;
using WindowsInput;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class turnButton : MonoBehaviour
{
  public Game managerRef;

  // Use this for initialization
  void Start()
  {
    managerRef = GameObject.Find("GameManager").GetComponent<Game>();
  }
  void Update()
  {

  }

  public void nextPlayerTurn()
  {
    StartCoroutine(pressSpace());
  }
  IEnumerator pressSpace()
  {
    InputSimulator.SimulateKeyDown(VirtualKeyCode.SPACE);
    yield return new WaitForSeconds(0.5f);
    InputSimulator.SimulateKeyUp(VirtualKeyCode.SPACE);
  }

  public void AttackList()
  {
    List<Unit> x = managerRef.attackList;
    x[0].Attack(x[1]);
    x = null;
  }

}
