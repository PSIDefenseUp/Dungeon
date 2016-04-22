using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DMBuildDisplay : MonoBehaviour
{
    private Game game;
    private GameObject buildDisplay;
    private Cursor cursor;
    private static Unit placing;

    private Text goldText;

    public List<Unit> placeable;
    public int gold;

    // Use this for initialization
    void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        buildDisplay = GameObject.Find("DMBuildDisplay");
        goldText = GameObject.Find("DMGold").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        buildDisplay.SetActive(game.building == true);

        // If we're placing a unit
        if(placing != null)
        {
            // Show a preview of the unit we're placing on the cursor
            placing.transform.position = cursor.transform.position;            

            // If we click and the tile under our cursor is open, BUILD UNIT THERE and we're done placing for now
            if(Input.GetMouseButtonDown(0) && game.map.getUnit(cursor.getPosition()) == null)
            {
                // BUILD UNIT
                if (canBuild(placing))
                {
                    buildUnit(placing, cursor.getPosition());

                    // We've placed the unit
                    placing = null;
                }                
            }

            // If we right click while trying to place, cancel
            if (Input.GetMouseButtonDown(1))
            {
                // Destroy the unit, we're not placing it
                Destroy(placing.gameObject);
                placing = null;
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

    public void setPlacing(int i)
    {
        if(placing != null)
            Destroy(placing.gameObject);

        placing = Instantiate(placeable[i]);
    }

    public static bool isPlacing()
    {
        return placing != null;
    }

    public bool canBuild(Unit u)
    {
        if (u.cost > gold)
            return false;

        return true;
    }

    void buildUnit(Unit u, Point p)
    {
        gold -= u.cost;
        spawnUnit(u, p);
    }

    public void spawnUnit(Unit u, Point p)
    {
        game.map.addUnit(p, u);
        u.transform.position = game.map.getTile(p).transform.position + new Vector3(0, 1, 0);
    }
}
