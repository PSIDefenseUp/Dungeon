using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DMBuildDisplay : NetworkBehaviour
{
    private Game game;
    private static Unit placing;
    [SyncVar]
    public int gold;
    
    [SyncVar]
    public int BuildingUnit;

    public List<Unit> placeable;

    public Cursor cursor;
    public Text goldText;
    public GameObject buildDisplay;
   

    // Use this for initialization
    void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        cursor = GetComponent<Cursor>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        buildDisplay.SetActive(game.building == true && cursor.netPlayer.myPlayerInfo.team == 1);

    if (!isLocalPlayer)
      return;

    // If we're placing a unit
    if (placing != null)
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
        BuildingUnit = i;
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
        
        if (isServer && isLocalPlayer)
        {
          Destroy(placing.gameObject);
          cursor.RpcBuild(BuildingUnit, p, game.map.getTile(p).transform.position + new Vector3(0, 1, 0));
        }
       else if(isLocalPlayer) 
        {
          cursor.CmdBuild(BuildingUnit, p, game.map.getTile(p).transform.position + new Vector3(0, 1, 0));
          game.map.addUnit(p, u);
        }
    }

    public void SetCursor(Cursor x)
    {
        cursor = x;
    }
}
