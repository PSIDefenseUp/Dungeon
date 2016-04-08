using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //private Text introTextObj;
    //private string introText;
    //private float charsPerSecond = 22;
    //private bool intro = true;
    //private float progress = 0;

    //public Texture bgMenu;
    //public Texture bgBlack;

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

    enum state
    {
        FADEIN,
        FADEOUT,
        DISPLAY,
        TITLE
    }

    // Use this for initialization
    void Start()
    {
        currentState = state.FADEIN;
        currentTexture = intro1;

        /*
        introTextObj = GameObject.Find("IntroText").GetComponent<Text>();
        introText = @"I met a traveller from an antique land
Who said: ''Two vast and trunkless legs of stone
Stand in the desert. Near them, on the sand,
Half sunk, a shattered visage lies, whose frown,
And wrinkled lip, and sneer of cold command,
Tell that its sculptor well those passions read
Which yet survive, stamped on these lifeless things,
The hand that mocked them and the heart that fed:
And on the pedestal these words appear:
'My name is Ozymandias, king of kings:
Look on my works, ye Mighty, and despair!'
Nothing beside remains. Round the decay
Of that colossal wreck, boundless and bare
The lone and level sands stretch far away.''";
        */
    }
	
	// Update is called once per frame
	void Update()
    {
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
                        SceneManager.LoadScene("Level1");
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

        /*
        if (intro)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgBlack);
            introTextObj.text = introText.Substring(0, (int)progress);
        }
        else
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgMenu);
            introTextObj.text = "";
        }
        */        
    }
}
