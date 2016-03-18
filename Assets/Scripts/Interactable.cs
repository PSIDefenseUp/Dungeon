using UnityEngine;
using System.Collections;

public class Interactable : Unit
{
    public void getInteracted(Unit other)
    {
        if (this.tag.Equals("Chest"))
        {

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
