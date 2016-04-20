using UnityEngine;
using System.Collections.Generic;

public class DialogDisplay : MonoBehaviour
{
    private static Game game;
    private static string currentLine; // The current line of dialog being drawn
    private static float x, y; // The position the string should be drawn at
    private static float alpha; // Alpha to draw the string with
    private static float duration; // How many seconds should the line show for?
    private static float height; // How high should it rise up?
    private static float progress; // How much time has passed?
    private static Unit speaker; // Which unit is speaking?

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();

        alpha = 1;
        duration = 3;
        height = 140;
        progress = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(currentLine != null && speaker != null)
        {
            Vector3 pos = game.gameCamera.WorldToScreenPoint(speaker.transform.position + new Vector3(-1f, 1f, -.25f));
            x = pos.x;
            y = Screen.height - pos.y - (height * progress/duration);
            alpha =  1 - progress / duration;
            progress += Time.deltaTime;
            if(progress > duration)
            {
                currentLine = null;
                alpha = 0;
                progress = 0;
            }
        }
        else
        {
            speaker = null;
            currentLine = null;
        }
	}

    void OnGUI()
    {
        GUI.color = new Color(1, 1, 1, alpha);
        GUI.Label(new Rect(x, y, 300, 100), currentLine);
    }

    public static void speak(Unit u, string s)
    {
        currentLine = s;
        speaker = u;        
        alpha = 1;
        progress = 0;

        Vector3 pos = game.gameCamera.WorldToScreenPoint(speaker.transform.position + new Vector3(-1f, 1f, -.25f));
        x = pos.x;
        y = Screen.height - pos.y;
    }    
}
