using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchItem : MonoBehaviour
{
    [SerializeField]
    private Transform castingBar;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "x0";
        }
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "x" + Inventory.FindObjectOfType<Inventory>().itemCount[transform.GetChild(i).GetComponent<Item>().id].ToString();
        }
    }

    public void SwitchItems(int itemIndex)
    {
        for (int i =0; i< transform.childCount; i++)
        {
            if (i == itemIndex)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void QuickCastItems(int itemIndex)
    {
        if (Inventory.FindObjectOfType<Inventory>().itemCount[transform.GetChild(itemIndex).GetComponent<Item>().id] > 0)
        {
            castingBar.GetComponent<CastingBar>().castCoroutine = castingBar.GetComponent<CastingBar>().StartCoroutine(castingBar.GetComponent<CastingBar>().Progress());
            StartCoroutine(FinishCast(itemIndex));

            castingBar.GetComponent<CastingBar>().castIcon.GetComponent<Image>().sprite = transform.GetChild(itemIndex).GetComponent<Item>().icon;
            castingBar.GetComponent<CastingBar>().castIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            if (transform.GetChild(itemIndex).GetComponent<Item>().type == "Powerup")
            {
                castingBar.GetComponent<CastingBar>().iconSize.sizeDelta = new Vector2(15, 30);
            }
            else if (transform.GetChild(itemIndex).GetComponent<Item>().type == "Potion")
            {
                castingBar.GetComponent<CastingBar>().iconSize.sizeDelta = new Vector2(30, 30);
            }
        }  
    }

    private IEnumerator FinishCast(int itemIndex)
    {
        yield return new WaitForSeconds(1.5f);
        if (castingBar.GetChild(0).GetComponent<Image>().fillAmount >= 0.98)
        {
            transform.GetChild(itemIndex).GetComponent<Item>().ItemUsage();
            Inventory.FindObjectOfType<Inventory>().itemCount[transform.GetChild(itemIndex).GetComponent<Item>().id]--;
        }
    }
}
