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

    enum result
    {
        UNIT,
        WALL,
        DESTROY
    };

    state currentState = state.SEARCH;
    result chargeResult;
    Point chargeDest;
    Point checking;
    Unit target;
    
    void performAction()
    {
        Debug.Log("PERFORMING AI " + currentState); 

        switch(currentState)
        {
            case state.SEARCH:
                // search in straight lines from this unit until we hit a wall/nulltile/hero
                chargeDest = new Point(this.position.x, this.position.y);                

                // NORTH
                checking = new Point(this.position.x, this.position.y);
                for (int y = Mathf.Max(this.position.y - 1, 0); y >= 0; y--)
                {
                    checking.y = y;
                    
                    if(game.map.getUnit(checking) != null && game.map.getUnit(checking).team == 0)
                    {
                        Debug.Log("UNIT FOUND AT " + checking.ToString());
                        chargeDest = new Point(checking.x, checking.y);
                        currentState = state.CHARGING;
                        break;
                    }
                }

                // SOUTH
                checking = new Point(this.position.x, this.position.y);
                for (int y = Mathf.Min(this.position.y + 1, (int)game.map.getBounds().height -1); y < game.map.getBounds().height; y++)
                {   
                    checking.y = y;

                    if (game.map.getUnit(checking) != null && game.map.getUnit(checking).team == 0)
                    {
                        Debug.Log("UNIT FOUND AT " + checking.ToString());
                        chargeDest = new Point(checking.x, checking.y);
                        currentState = state.CHARGING;
                        break;
                    }
                }

                // EAST
                checking = new Point(this.position.x, this.position.y);
                for (int x = Mathf.Min(this.position.x + 1, (int)game.map.getBounds().width - 1); x < game.map.getBounds().width; x++)
                {
                    checking.x = x;

                    if (game.map.getUnit(checking) != null && game.map.getUnit(checking).team == 0)
                    {
                        Debug.Log("UNIT FOUND AT " + checking.ToString());
                        chargeDest = new Point(checking.x, checking.y);
                        currentState = state.CHARGING;
                        break;
                    }
                }

                // WEST
                checking = new Point(this.position.x, this.position.y);
                for (int x = Mathf.Max(this.position.x - 1, 0); x >= 0; x--)
                {
                    checking.x = x;

                    if (game.map.getUnit(checking) != null && game.map.getUnit(checking).team == 0)
                    {
                        Debug.Log("UNIT FOUND AT " + checking.ToString());
                        chargeDest = new Point(checking.x, checking.y);
                        currentState = state.CHARGING;
                        break;
                    }
                }

                break;

            case state.CHARGING:
                // Find farthest position we can charge to, then move there and get to the correct state based on what stopped the charge
                if (chargeDest.y > this.position.y)
                {
                    // Set current working 'destination' as current loc
                    checking = new Point(this.position.x, this.position.y);

                    // Loop through map to find farthest loc that we can reach
                    for (int y = this.position.y; y < game.map.getBounds().height; y++)
                    {
                        Debug.Log(y);
                                                
                        // Check to see if this space has a pathable tile. If not, end here at the "wall"
                        if (game.map.getTile(checking) != null && !(game.map.getTile(checking).solid))
                        {
                            // If there isn't a unit, we can travel here, otherwise we need to stop and determine what happens next
                            if(game.map.getUnit(new Point(checking.x, y)) == null)
                            {
                                checking = new Point(checking.x, y);
                            }
                            else
                            {
                                Interactable other = game.map.getUnit(new Point(checking.x, y)) as Interactable;

                                if(other != null && other.tag.Equals("Destructible"))
                                {
                                    chargeDest = new Point(checking.x, checking.y);
                                    chargeResult = result.DESTROY;
                                    target = game.map.getUnit(new Point(checking.x, y));
                                    break;
                                }
                                else
                                {
                                    chargeDest = new Point(checking.x, checking.y);
                                    chargeResult = result.UNIT;
                                    target = game.map.getUnit(new Point(checking.x, y));
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Wall");
                            chargeDest = new Point(checking.x, checking.y);
                            chargeResult = result.WALL;
                            break;
                        }
                    }
                }                

                Debug.Log("reached");
                moveTo(chargeDest);
                canMove = false;
                canAct = false;

                switch(chargeResult)
                {
                    case result.DESTROY:
                        // destroy other unit and goto incap
                        game.map.removeUnit(target);
                        currentState = state.INCAP;
                        break;

                    case result.UNIT:
                        // destroy the other unit
                        game.map.removeUnit(target);
                        currentState = state.SEARCH;
                        break;

                    default:
                        // get stunned
                        currentState = state.SEARCH;
                        break;
                }                
                break;

            case state.STUN:
                //canMove = false;
                //canAct = false;
                //currentState = state.SEARCH;
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
