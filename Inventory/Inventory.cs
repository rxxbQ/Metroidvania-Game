using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private bool inventoryEnabled;

    [SerializeField]
    private GameObject inventory;

    [SerializeField]
    private int allSlots;
    private GameObject[] slot;

    [SerializeField]
    private GameObject slotHolder;

    public Dictionary<int, int> itemCount;

    // Start is called before the first frame update
    void Start()
    {
        itemCount = new Dictionary<int, int>();
        slot = new GameObject[allSlots];

        for (int i =0; i< allSlots; i++)
        {
            itemCount.Add(i, 0);
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryEnabled = !inventoryEnabled;
        }
        if (inventoryEnabled)
        {
            inventory.SetActive(true);
        }
        else
        {
            inventory.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "item")
        {
            GameObject itemPickedUp = other.gameObject;
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.id, item.type, item.des, item.icon);
        }
    }

    private void AddItem(GameObject itemObject, int id, string type, string des, Sprite icon)
    {
        if (itemCount.ContainsKey(id))
        {
            itemCount[id]++;
        }

        for (int i = 0; i < allSlots; i++)
        {
            if (slot[i].GetComponent<Slot>().icon == icon)
            {
                //slot[i].GetComponent<Slot>().StackCount++;
                
                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                return;
            }
            else if (slot[i].GetComponent<Slot>().empty)
            {
                //itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().icon = icon;
                slot[i].GetComponent<Slot>().id = id;
                slot[i].GetComponent<Slot>().type = type;
                slot[i].GetComponent<Slot>().des = des;
                
                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;

                return;
            }
        }
    }
}
