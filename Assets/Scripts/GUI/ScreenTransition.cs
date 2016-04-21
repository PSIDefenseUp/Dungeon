using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public Texture black;

    private Texture fadeTexture;
    private float progress = 0;
    private float fadeTime = 0;
    private float alpha = 0;
    public bool fading = false;

	// Use this for initialization
	void Start ()
    {
        fadeTexture = black;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (fading)
        {
            progress += Time.deltaTime;

            if(progress / fadeTime >= 1)
            {
                progress = 0;
                alpha = 1;
                fading = false;                
            }
            else
            {
                alpha = progress / fadeTime;
            }            
        }
	}

    void OnGUI()
    {
        if (fadeTexture != null)
        {
            GUI.color = new Color(1, 1, 1, alpha);

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }

    public void fadeToBlack(float seconds)
    {
        fadeToTexture(black, seconds);
    }

    public void fadeToTexture(Texture texture, float seconds)
    {
        fadeTexture = texture;
        fadeTime = seconds;
        alpha = 0;
        fading = true;        
    }

    public void fadeFromTexture(Texture newTexture, float seconds)
    {
        
    }
}
