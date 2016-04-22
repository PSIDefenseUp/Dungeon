using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialDisplay : MonoBehaviour
{
    public List<string> messages;

    private GameObject tutorialWindow;
    private Text tutorialText;
    private int tutorialProgress;
    private GameObject gui;

	// Use this for initialization
	void Start ()
    {
        tutorialWindow = GameObject.Find("TutorialWindow");
        tutorialText = GameObject.Find("TutorialText").GetComponent<Text>();
        gui = GameObject.Find("GUI");

        showTutorial();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}    

    public void rewindTutorial()
    {
        if (tutorialProgress == 0)
            return;

        tutorialProgress--;
        tutorialProgress = tutorialProgress % messages.Count;
        tutorialText.text = "" + messages[tutorialProgress];
    }

    public void advanceTutorial()
    {
        tutorialProgress++;
        tutorialProgress = tutorialProgress % messages.Count;
        tutorialText.text = "" + messages[tutorialProgress];

        if (tutorialProgress == 0)
            hideTutorial();
    }

    public void showTutorial()
    {
        tutorialWindow.SetActive(true);
        tutorialProgress = 0;
        tutorialText.text = "" + messages[tutorialProgress];
        gui.GetComponentInChildren<HealthBarDisplay>().active = false;
    }

    public void hideTutorial()
    {
        tutorialWindow.SetActive(false);
        gui.GetComponentInChildren<HealthBarDisplay>().active = true;
    }

    public void toggleTutorial()
    {
        if(tutorialWindow.activeSelf)
        {
            hideTutorial();
        }
        else
        {
            showTutorial();
        }
    }

    public int getTutorialProgress()
    {
        return tutorialProgress;
    }

    public int getTutorialLength()
    {
        return messages.Count;
    }
}
