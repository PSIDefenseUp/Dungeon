using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    private string sceneToLoad; // The scene we will be loading in
    private float progress;
    private AsyncOperation sceneLoad;
    private bool loading = false;

    // UI Elements
    private Image loadBar;
    private Image loadBG;
    private Image background;

    // Use this for initialization
    void Start ()
    {
        loadBar = GameObject.Find("LoadBar").GetComponent<Image>();
        loadBG = GameObject.Find("LoadBG").GetComponent<Image>();
        background = GameObject.Find("Background").GetComponent<Image>();
    }
	
    void OnGUI()
    {       
        if(!loading)
            return;
                
        progress = sceneLoad.progress;
        loadBar.rectTransform.sizeDelta = new Vector2(loadBG.rectTransform.sizeDelta.x * progress, loadBar.rectTransform.sizeDelta.y);        
    }
    
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void loadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        loading = true;
        progress = 0;
        sceneLoad = SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
