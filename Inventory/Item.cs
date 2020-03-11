using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int id;
    public string type;
    public string des;
    public Sprite icon;

    public void ItemUsage()
    {
        if (type == "Powerup")
        {
            if (id == 2)
            {
                Player.Instance.hp.MaxValue = 0.5f * Player.Instance.hp.MaxValue;
                Player.Instance.hp.CurrentValue = Player.Instance.hp.MaxValue;
                Player.Instance.attackDamage = 2 * Player.Instance.attackDamage;
            }
            if (id == 3)
            {
                Player.Instance.attackDamage = (int)Mathf.Round(1.25f * Player.Instance.attackDamage);
            }
            if (id == 4)
            {
                Player.Instance.defence = (int)Mathf.Round(1.25f * Player.Instance.defence);
            }
        }
        if (type == "Potion")
        {
            if (id == 0)
            {
                Player.Instance.hp.CurrentValue += 15;
            }
            if (id == 1)
            {
                Player.Instance.mana.CurrentValue += 15;
            }
        }
    }
}
