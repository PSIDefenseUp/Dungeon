using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Manager : NetworkManager
{
  private InitGameSequence init;
  // Use this for initialization
  void Start ()
  {
    //init = GameObject.Find("GameManager").GetComponent<InitGameSequence>();
  }
	
	// Update is called once per frame
	void Update () {
	
	}

  /*public override void OnClientConnect(NetworkConnection conn)
  {
    base.OnClientConnect(conn);
   // init.setTeam(true);
  }
  */

}

