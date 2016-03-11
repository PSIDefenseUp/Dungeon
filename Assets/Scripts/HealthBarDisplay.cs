using UnityEngine;
using System.Collections;

public class HealthBarDisplay : MonoBehaviour
{
    private Game game;

    public Texture red;
    public Texture green;

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
            Vector3 pos = game.gameCamera.WorldToScreenPoint(u.transform.position + new Vector3(-.5f, 1, .5f));
            GUI.DrawTexture(new Rect(pos.x, Screen.height - pos.y, 100, 10), red);
            GUI.DrawTexture(new Rect(pos.x, Screen.height - pos.y, 100 * (u.currentHealth / (float)u.maxHealth), 10), green);
        }
    }
}
