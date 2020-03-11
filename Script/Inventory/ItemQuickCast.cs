using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuickCast : MonoBehaviour
{
    [SerializeField]
    private Transform upItem;

    [SerializeField]
    private Transform downItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            upItem.GetComponent<SwitchItem>().QuickCastItems(Player.Instance.upIndex);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            downItem.GetComponent<SwitchItem>().QuickCastItems(Player.Instance.downIndex);
        }
    }
}
