﻿using UnityEngine;
using WindowsInput;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class turnButton : MonoBehaviour
{
    public Game managerRef;
    public List<Unit> uList;
    public Unit unitAI;

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
    managerRef.map.getUnitList();
    uList = managerRef.map.getUnitList();

    unitAI = uList[3];
    managerRef.cursor.selectUnit(unitAI);

    }
}
