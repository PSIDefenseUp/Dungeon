using UnityEngine;
using System.Collections;

public class Item
{
    public int bonusRegen;
    public int bonusArmor;

    public int bonusAttack;
    public int bonusRange;

    public int bonusSpeed;

    public Item()
    {
        this.generate();
    }

    public void generate()
    {
        bonusRegen = 0;
        bonusArmor = 0;
        bonusAttack = 0;
        bonusRange = 0;
        bonusSpeed = 0;

        switch ((int)(Random.value * 4))
        {
            case 0: bonusRegen = (int)(Random.value * 4) + 1; break;
            case 1: bonusArmor = (int)(Random.value * 2) + 1; break;
            case 2: bonusAttack = (int)(Random.value * 6) + 1; break;
            case 3: bonusRange = (int)(Random.value * 1) + 1; break;
            case 4: bonusSpeed = (int)(Random.value * 2) + 1; break;
        }
    }
}
