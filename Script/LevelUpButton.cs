using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpButton : MonoBehaviour
{
    public void LevelUp(int index)
    {
        Player.Instance.Level++;
        GameManager.Instance.Gold -= Player.Instance.RequiredCoin;
        Player.Instance.RequiredCoin = (int)Mathf.Round(Player.Instance.RequiredCoin * 1.1f);

        if (index == 0)
        {
            Player.Instance.hp.MaxValue += 5;
            Player.Instance.hp.CurrentValue = Player.Instance.hp.MaxValue;
        }
        if (index == 1)
        {
            Player.Instance.mana.MaxValue += 4;
            Player.Instance.mana.CurrentValue = Player.Instance.mana.MaxValue;
        }
        if (index == 2)
        {
            Player.Instance.stamina.MaxValue += 4;
            Player.Instance.stamina.CurrentValue = Player.Instance.stamina.MaxValue;
        }
        if (index == 3)
        {
            Player.Instance.attackDamage += 4;
        }
        if (index == 4)
        {
            Player.Instance.defence += 4;
        }
        if (index == 5)
        {
            Player.Instance.Luck += 4;
        }
    }
}
