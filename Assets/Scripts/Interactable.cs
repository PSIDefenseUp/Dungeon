﻿using UnityEngine;
using System.Collections;

public class Interactable : Unit
{
    public void getInteracted(Unit other)
    {
        if (this.tag.Equals("TrapChest"))
        {
            // Get reference to map
            Map map = GameObject.Find("GameManager").GetComponent<Game>().map;

            // Get map bounds (for width and height)
            Rect mapBounds = map.getBounds();

            // Get reference to unit builder
            DMBuilder builder = GameObject.Find("DMBuilder").GetComponent<DMBuilder>();

            for(int i = 0; i < 5;)
            {
                Point p = new Point(0, 0);

                p.x = (int)(Random.value * (mapBounds.width - 1));
                p.y = (int)(Random.value * (mapBounds.height - 1));

                if(map.getTile(p) != null && !map.getTile(p).solid && map.getUnit(p) == null)
                {
                    Unit u = Instantiate(builder.spawnable);
                    builder.spawnUnit(u, p);
                    i++;
                }
            }
        }
        else if (this.tag.Equals("Key"))
        {
            game.currentPlayer.keyCount++;
        }
        else if (this.tag.Equals("Gate"))
        {
            if (game.currentPlayer.keyCount == 0)
                return;

            game.currentPlayer.keyCount--;
        }

        game.map.removeUnit(this);
    }
}