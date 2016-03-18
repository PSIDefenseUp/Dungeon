using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyDisplay : MonoBehaviour
{
    private Game game;

    public Text keyText;

    // Use this for initialization
    void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        keyText = GameObject.Find("HeroKeys").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnGUI()
    {
        // Show how many keys the heroes have during their turn(s)
        if (game.currentPlayer.team == 0)
            keyText.text = "KEYS HELD: " + game.currentPlayer.keyCount;
        else
            keyText.text = "";
    }
}
