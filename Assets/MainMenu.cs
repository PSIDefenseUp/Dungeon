using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string introText;

    private float charsPerSecond = 50;
    private bool intro = true;
    private float progress = 0;
    private GUIStyle style;

    // Use this for initialization
    void Start()
    {
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
Nothing beside remains.Round the decay
Of that colossal wreck, boundless and bare
The lone and level sands stretch far away.''";
    }
	
	// Update is called once per frame
	void Update()
    {
        if(intro)
        {
            progress += charsPerSecond * Time.deltaTime;
            if (progress > introText.Length)
                progress = introText.Length;
        }

	    if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (intro)
            {
                if (progress < introText.Length)
                    progress = introText.Length;
                else
                    intro = false;
            }
                
            else
                SceneManager.LoadScene("Level1");
        }
	}

    void OnGUI()
    {
        if(intro)
            GUI.Label(new Rect(0, 0, Screen.width, Screen.height), introText.Substring(0, (int)progress));
    }
}
