using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteraction
{
    [SerializeField]
    private Sprite openedChest;
    private bool opened = false;

    public void Interact()
    {
        GetComponent<SpriteRenderer>().sprite = openedChest;
        
        if (opened == false)
        {
            FindObjectOfType<AudioManager>().Play("openChest");
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.45f, transform.position.z);
        }
        opened = true;
    }

}
