using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEnd : MonoBehaviour
{
    public Texture heroWinTexture;
    public Texture dmWinTexture;

    private Game game;

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void endGame(int winningTeam)
    {
        game.gameOver = true;        

        if (winningTeam == 0)
        {
            // Heroes win!
            //Debug.Log("HEROES WIN!");
            GameObject.Find("GUI").GetComponent<ScreenTransition>().fadeToTexture(heroWinTexture, 1);
        }
        else if (winningTeam == 1)
        {
            // DM wins!
            //Debug.Log("DM WINS!");
            
            GameObject.Find("GUI").GetComponent<ScreenTransition>().fadeToTexture(dmWinTexture, 1);
        }

        GameObject.Find("GUI").GetComponent<HealthBarDisplay>().active = false;
    }
}
