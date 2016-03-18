using UnityEngine;
using System.Collections;

public class AIUnit : Unit
{
    enum state
    {
        SEARCH,
        CHARGING,
        STUN,
        INCAP
    };

    state currentState = state.SEARCH;
    Point chargeDest;

	void performAction()
    {
        Debug.Log("PERFORMING AI " + currentState); 

        switch(currentState)
        {
            case state.SEARCH:
                // search in straight lines from this unit until we hit a wall/nulltile/hero
                chargeDest = this.position;
                Point checking;
                
                break;

            case state.CHARGING:
                moveTo(chargeDest);
                canMove = false;
                canAct = false;
                currentState = state.STUN;
                break;

            case state.STUN:
                canMove = false;
                canAct = false;
                currentState = state.SEARCH;
                break;

            case state.INCAP:
                canMove = false;
                canAct = false;
                currentState = state.INCAP;
                break;

            default:
                break;
        }
    }

    public override void refresh()
    {
        // To be called on the start of its owner's turn
        // Allows the unit to act and move again, and brings back its light
        canMove = true;
        canAct = true;
        this.heal(this.regen);

        performAction();
    }
}
