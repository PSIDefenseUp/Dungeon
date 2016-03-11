using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DMBuilder : MonoBehaviour
{
    private Game game;

    public int gold;
    public Unit spawnable;
    public Text goldText;

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        goldText = GameObject.Find("DMGold").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the DM is going, let him BUILD
	    if(game.currentPlayer.team == 1)
        {
            // If Q is pressed, try to build a unit on the current space
            if(Input.GetKeyDown(KeyCode.Q))
            {
                if (canBuild(spawnable, game.cursor.getPosition()))
                {
                    Unit u = Instantiate(spawnable);
                    buildUnit(u, game.cursor.getPosition());
                }
            }
        }
	}

    void OnGUI()
    {
        if (game.currentPlayer.team == 1)
            goldText.text = "DM GOLD: " + gold;
        else
            goldText.text = "";
    }
        
    public bool canBuild(Unit u, Point p)
    {
        if (u.cost > gold)
            return false;

        if (game.map.getUnit(p) != null)
            return false;

        return true;
    }

    void buildUnit(Unit u, Point p)
    {
        gold -= u.cost;
        game.map.addUnit(p, u);
        u.transform.position = game.map.getTile(p).transform.position + new Vector3(0, 1, 0);
    }
}
