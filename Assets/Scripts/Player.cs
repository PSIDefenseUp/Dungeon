using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

  public string playerName;   // Player Name
  public List<Unit> units;    // List of Units on the board the player controls


  public bool isTurn = false; // boolean for later use of highlighting player units during their turn



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
