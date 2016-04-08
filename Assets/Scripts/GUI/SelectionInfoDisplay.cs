using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectionInfoDisplay : MonoBehaviour
{
    private Cursor cursor; // Cursor whose selection info we will be displaying
    private GameObject panel;
    private CanvasGroup panelGroup;

    private Unit unit;

    private Text nameText;
    private Text health;
    private Text regen;
    private Text armor;
    private Text attack;
    private Text range;
    private Text speed;
    private Text canMove;
    private Text canAct;
    
	void Start ()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Cursor>() ;

        panel = GameObject.Find("SelectionInfoPanel");
        panelGroup = panel.GetComponent<CanvasGroup>();

        nameText = GameObject.Find("NameLabel").GetComponent<Text>();

        health = GameObject.Find("HealthLabel").GetComponent<Text>();
        regen = GameObject.Find("RegenLabel").GetComponent<Text>();
        armor = GameObject.Find("ArmorLabel").GetComponent<Text>();

        attack = GameObject.Find("AttackLabel").GetComponent<Text>();
        range = GameObject.Find("RangeLabel").GetComponent<Text>();
        speed = GameObject.Find("SpeedLabel").GetComponent<Text>();

        canMove = GameObject.Find("MoveLabel").GetComponent<Text>();
        canAct = GameObject.Find("ActLabel").GetComponent<Text>();        
    }
	
	void Update ()
    {
        unit = cursor.getSelection();

        panelGroup.alpha = (unit == null ? 0 : 1);        

        if (unit == null)
            return;

        nameText.text = "Selected: " + unit.name;

        health.text = "HP: " + unit.currentHealth + " / " + unit.maxHealth;
        regen.text = "Regen: " + unit.getRegen();
        armor.text = "Armor: " + unit.getArmor();

        attack.text = "Attack: [" + unit.getAttackBase() + ", " + (unit.getAttackBase() + unit.attackSpread) + "]";
        range.text = "Range: [" + unit.getMinRange() + ", " + unit.getMaxRange() + "]";
        speed.text = "Speed: " + unit.getMoveSpeed();

        canMove.text = "Can Move: " + (unit.canMove ? "YES" : "NO");
        canAct.text = "Can Act: " + (unit.canAct ? "YES" : "NO");
    }
}
