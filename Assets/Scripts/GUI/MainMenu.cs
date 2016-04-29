using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private float fadeTime = 1.0f;    // How long should each image take to fade in/out?
    private float displayTime = 9f;  // How long do we display each intro image between fading?
    private int introProgress = 0;  // Which intro image are we showing?
    private float progress = 0;     // How far we are in seconds into our current state
    private state currentState;     // What state are we in now?
    private Texture currentTexture;
    private float currentAlpha = 0;

    public Texture intro1;
    public Texture intro2;
    public Texture intro3;
    public Texture bgMenu;

    public LoadScreen loadScreen;

    enum state
    {
        FADEIN,
        FADEOUT,
        DISPLAY,
        TITLE,
    }

    // Use this for initialization
    void Start()
    {
        loadScreen = GameObject.Find("LoadScreen").GetComponent<LoadScreen>();
        loadScreen.gameObject.SetActive(false);

        currentState = state.FADEIN;
        currentTexture = intro1;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (loadScreen.isActiveAndEnabled)
            return;

        // Get correct intro image
        switch (introProgress)
        {
            case 0: currentTexture = intro1; break;
            case 1: currentTexture = intro2; break;
            case 2: currentTexture = intro3; break;
            default:
                currentTexture = bgMenu;
                currentAlpha = 1;
                currentState = state.TITLE;
                break;
        }

        // update state timer and manage fading, etc
        switch (currentState)
        {          
            case state.FADEIN:
                currentAlpha = progress / fadeTime;
                progress += Time.deltaTime;
                if(progress/fadeTime >= 1)
                {
                    currentState = state.DISPLAY;
                    progress = progress % fadeTime;
                }
                break;

            case state.FADEOUT:
                currentAlpha =  1 - (progress / fadeTime);
                progress += Time.deltaTime;
                if (progress / fadeTime >= 1)
                {
                    currentState = state.FADEIN;
                    introProgress++;
                    progress = progress % fadeTime;
                }
                break;

            case state.DISPLAY:
                currentAlpha = 1;
                progress += Time.deltaTime;
                if (progress / displayTime >= 1)
                {
                    currentState = state.FADEOUT;
                    progress = progress % fadeTime;
                }
                break;

            default: break;
        }

                
	    if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                switch (currentState)
                {
                    case state.TITLE:
                        currentAlpha = 0;
                        loadScreen.gameObject.SetActive(true);
                        loadScreen.loadScene("Level1Network");
                        break;

                    default:
                        progress = 0;
                        introProgress++;
                        break;
                }
            }
        }
	}

    void OnGUI()
    {
        GUI.color = new Color(1, 1, 1, currentAlpha);

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), currentTexture);        
    }
}
