using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectionInventoryDisplay : MonoBehaviour
{
    private Game game;
    private GameObject inventoryDisplay;
    private Inventory inventory;

    
    public Sprite occupiedSlot;
    public Sprite emptySlot;
    public networkPlayerScript myPlayer;
    public Cursor cursor;

  // Use this for initialization
  void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        inventoryDisplay = GameObject.Find("SelectionInventoryDisplay");
    }
	
	// Update is called once per frame
	void Update ()
    { 
        // Draw inventory display if you are have a hero selected and have a hero selected
        if (myPlayer.myPlayerInfo.team == 1 && cursor.getSelection() != null && cursor.getSelection().team == 0)
        {
            inventoryDisplay.SetActive(true);

            inventory = cursor.getSelection().inventory;

            // Draw icons for slots with items
            if (cursor.getSelection().inventory.numItems == 0)
                return;
            else
            {
                // Slot 0
                if(inventory.items[0] != null)
                    GameObject.Find("Slot1BG").GetComponent<Image>().sprite = occupiedSlot;
                else
                    GameObject.Find("Slot1BG").GetComponent<Image>().sprite = emptySlot;

                // Slot 1
                if (inventory.items[1] != null)
                    GameObject.Find("Slot2BG").GetComponent<Image>().sprite = occupiedSlot;
                else
                    GameObject.Find("Slot2BG").GetComponent<Image>().sprite = emptySlot;

                // Slot 2
                if (inventory.items[2] != null)
                    GameObject.Find("Slot3BG").GetComponent<Image>().sprite = occupiedSlot;
                else
                    GameObject.Find("Slot3BG").GetComponent<Image>().sprite = emptySlot;

                // Slot 3
                if (inventory.items[3] != null)
                    GameObject.Find("Slot4BG").GetComponent<Image>().sprite = occupiedSlot;
                else
                    GameObject.Find("Slot4BG").GetComponent<Image>().sprite = emptySlot;
            }
        }
        else
        {
            inventoryDisplay.SetActive(false);
        }
    }

    public void tooltip(int i)
    {
        inventory.tooltip(i);
    }
    
    public void SetCursor(Cursor x)
    {
        cursor = x;
    }

}
