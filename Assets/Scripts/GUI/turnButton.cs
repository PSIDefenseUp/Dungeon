using UnityEngine;
using WindowsInput;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class turnButton : MonoBehaviour
{
    public Game managerRef;

    // Use this for initialization
    void Start()
    {
        managerRef = GameObject.Find("GameManager").GetComponent<Game>();
    }

    void Update()
    {

    }

    public void nextPlayerTurn()
    {
        managerRef.advanceTurn();
    }
}
