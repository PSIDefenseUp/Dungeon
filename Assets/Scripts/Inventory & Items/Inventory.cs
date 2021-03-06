﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory
{
    public Item[] items;
    public int numItems = 0;

    public Inventory()
    {
        items = new Item[4];
        numItems = 0;
    }

    public int getBonusRegen()
    {
        int bonusRegen = 0;

        foreach (Item i in items)
        {
            if (i != null)
                bonusRegen += i.bonusRegen;
        }

        return bonusRegen;
    }

    public int getBonusArmor()
    {
        int bonusArmor = 0;

        foreach (Item i in items)
        {
            if(i != null)
                bonusArmor += i.bonusArmor;
        }

        return bonusArmor;
    }

    public int getBonusAttack()
    {
        int bonusAttack = 0;

        foreach (Item i in items)
        {
            if (i != null)
                bonusAttack += i.bonusAttack;
        }

        return bonusAttack;
    }

    public int getBonusRange()
    {
        int bonusRange = 0;

        foreach (Item i in items)
        {
            if (i != null)
                bonusRange += i.bonusRange;
        }

        return bonusRange;
    }

    public int getBonusSpeed()
    {
        int bonusSpeed = 0;

        foreach (Item i in items)
        {
            if (i != null)
                bonusSpeed += i.bonusSpeed;
        }

        return bonusSpeed;
    }

    public void addItem(Item item)
    {
        if (isFull())
            return;

        for (int i = 0; i < 4; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                numItems++;
                return;
            }
        }
    }

    public void removeItem(int i)
    {
        items[i] = null;
        numItems--;

        if (numItems < 0)
            numItems = 0;
    }

    public bool isFull()
    {
        return numItems == 4;
    }

    public void tooltip(int i)
    {
        // If there's an item in this slot, show a tooltip. Otherwise show empty.
        GameObject.Find("InventoryTooltipText").GetComponent<Text>().text = (items[i] != null ? items[i].ToString() : "EMPTY");
    }
}
