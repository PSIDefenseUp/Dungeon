using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectionInventoryDisplay : MonoBehaviour
{
    private Game game;

    private Cursor cursor;
    private GameObject inventoryDisplay;
    private Inventory inventory;

    public Sprite occupiedSlot;
    public Sprite emptySlot;

	// Use this for initialization
	void Start ()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>();
        inventoryDisplay = GameObject.Find("SelectionInventoryDisplay");
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Draw inventory display if we have a hero selected
        if (game.currentPlayerIndex == 0 && cursor.getSelection() != null && cursor.getSelection().team == 0)
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
}
