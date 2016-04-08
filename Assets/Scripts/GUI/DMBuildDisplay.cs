using UnityEngine;
using System.Collections;

public class DMBuildDisplay : MonoBehaviour
{
    private Game game;
    private GameObject buildDisplay;

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        buildDisplay = GameObject.Find("DMBuildDisplay");
    }
	
	// Update is called once per frame
	void Update ()
    {
        buildDisplay.SetActive(game.currentPlayerIndex == 1);
	}
}
