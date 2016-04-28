using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InitGameSequence : MonoBehaviour {

  private bool Name = true;
  private GameObject initP;
  private GameObject initT;
  private InputField name;
  private Dropdown team;
  private NetworkManagerHUD x;

  public string myName;
  public Game.player myPlayer;
  public int myTeam;



	// Use this for initialization
	void Start ()
  {
    initT = GameObject.Find("PlayerTeam");
    team = initT.GetComponentInChildren<Dropdown>();
    initT.SetActive(false);

    initP = GameObject.Find("PlayerName");
    name = initP.GetComponentInChildren<InputField>();

    x = GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>();
    x.showGUI = false;
  }
	
	// Update is called once per frame
	void Update ()
  {

    if (Name)
    {
      initP.SetActive(true);
    }   	
	}

  public void setName()
  {
    if (name.text.ToString() == "") return;
    myName = name.text.ToString();
    Name = false;
    name.gameObject.SetActive(false);
    initP.GetComponentInChildren<Button>().gameObject.SetActive(false);
    initT.SetActive(true);

  }

  public void setTeam()
  {
    if (team.value == 0)
    {
      myTeam = 0;
      myPlayer = new Game.player(myTeam, myName + " - Hero");
    }
    else
    {
      myTeam = 1;
      myPlayer = new Game.player(myTeam, myName + " - DM");
    }
    initT.SetActive(false);
    x.showGUI = true;
  }

}
