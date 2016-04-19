using UnityEngine;
using System.Collections;

public class HealthBarDisplay : MonoBehaviour
{
    private Game game;

    public Texture red;
    public Texture green;
    public Texture black;
    public Texture white;

    const int healthBarHeight = 10;

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnGUI()
    {
        foreach(Unit u in game.map.getUnitList())
        {
            if (u.maxHealth == 0)
                continue;

            // Top left
            Vector3 pos = game.gameCamera.WorldToScreenPoint(u.transform.position + new Vector3(-.4f, -.5f, -.25f));

            // Bottom right
            Vector3 pos2 = game.gameCamera.WorldToScreenPoint(u.transform.position + new Vector3(.4f, -.5f, -.25f));

            // Draw health bar 'outline' (white if can act, black otherwise)
            if(game.currentPlayer.team == u.team && u.canAct)
                GUI.DrawTexture(new Rect(pos.x - 2, Screen.height - pos.y - 2, pos2.x - pos.x + 4, healthBarHeight + 4), white);
            else
                GUI.DrawTexture(new Rect(pos.x - 2, Screen.height - pos.y - 2, pos2.x - pos.x + 4, healthBarHeight + 4), black);

            // Draw black background
            GUI.DrawTexture(new Rect(pos.x, Screen.height - pos.y, pos2.x - pos.x, healthBarHeight), black);

            // Draw health depending on the currently acting player
            if(game.currentPlayer.team == u.team)
                GUI.DrawTexture(new Rect(pos.x, Screen.height - pos.y, (pos2.x - pos.x) * (u.currentHealth / (float)u.maxHealth), healthBarHeight), green);
            else
                GUI.DrawTexture(new Rect(pos.x, Screen.height - pos.y, (pos2.x - pos.x) * (u.currentHealth / (float)u.maxHealth), healthBarHeight), red);
        }
    }
}
