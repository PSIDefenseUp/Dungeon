using UnityEngine;
using System.Collections;

public class turnButton : MonoBehaviour {

  public Game managerRef;
  public int minIndex = 0;
  public int maxIndex = 0;

  // Use this for initialization
  void Start ()
  {
    managerRef = GameObject.Find("GameManager").GetComponent<Game>();
	}
  void Update()
  {
    maxIndex = managerRef.getPlayerListSize();
  }

  public void nextPlayerTurn()
  {
    managerRef.currentPlayerIndex++;

    if(managerRef.currentPlayerIndex >= maxIndex)
    {
      managerRef.currentPlayerIndex =  0;
    }
  }
	
}
