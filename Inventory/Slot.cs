using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public Transform slotIcon;
    public int id;
    public string type;
    public string des;
    public bool empty;
    public Sprite icon;

    private int oldAttack;
    private float oldDefence;
    private float oldHealth;

    [SerializeField]
    private Transform castingBar;

    public Text Count { get; set; }

    private void Start()
    {
        Count = transform.GetChild(1).GetComponent<Text>();
        slotIcon = transform.GetChild(0);
        oldAttack = Player.Instance.attackDamage;
        oldDefence = Player.Instance.defence;
        oldHealth = Player.Instance.hp.MaxValue;
    }

    void Update()
    {
        if (item != null)
        {
            Count.text = "x" + Inventory.FindObjectOfType<Inventory>().itemCount[item.GetComponent<Item>().id].ToString();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            castingBar.GetComponent<CastingBar>().castIcon.GetComponent<Image>().sprite = icon;
            castingBar.GetComponent<CastingBar>().castIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (type == "Powerup")
            {
                castingBar.GetComponent<CastingBar>().iconSize.sizeDelta = new Vector2(15, 30);
            }
            else if (type == "Potion")
            {
                castingBar.GetComponent<CastingBar>().iconSize.sizeDelta = new Vector2(30, 30);
            }

            castingBar.GetComponent<CastingBar>().castCoroutine = castingBar.GetComponent<CastingBar>().StartCoroutine(castingBar.GetComponent<CastingBar>().Progress());
            StartCoroutine(FinishCast());
        }
    }

    public void UpdateSlot()
    {
        slotIcon.GetComponent<Image>().sprite = icon;      
        slotIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        slotIcon.GetComponent<Image>().SetNativeSize();
    }

    public void UseItem()
    {
        item.GetComponent<Item>().ItemUsage();
        StartCoroutine(Duration(item.GetComponent<Item>().id));
    }

    public void RemoveItem()
    {
        Inventory.FindObjectOfType<Inventory>().itemCount[item.GetComponent<Item>().id]--;
        if (Inventory.FindObjectOfType<Inventory>().itemCount[item.GetComponent<Item>().id] <= 0)
        {
            item = null;
            empty = true;
            Transform usedItem = transform.GetChild(1);
            usedItem.parent = null;
            slotIcon.GetComponent<Image>().color = new Color32(0, 0, 0, 100);
            slotIcon.GetComponent<Image>().sprite = null;
        }   
    }

    private IEnumerator FinishCast()
    {
        yield return new WaitForSeconds(1.5f);
        if (castingBar.GetChild(0).GetComponent<Image>().fillAmount >= 0.98)
        {
            UseItem();
            RemoveItem();
        }
    }

    public IEnumerator Duration(int id)
    {
        yield return new WaitForSeconds(30.0f);

        if (id == 2)
        {
            Player.Instance.hp.MaxValue = oldHealth;
            Player.Instance.attackDamage = oldAttack;
        }
        if (id == 3)
        {
            Player.Instance.attackDamage = oldAttack;
        }
        if (id == 4)
        {
            Player.Instance.defence = oldDefence;
        }
    }
}
