using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteraction
{
    [SerializeField]
    private Sprite openedChest;
    private bool opened = false;

    [SerializeField]
    private bool dropLeft;

    public void Interact()
    {
        GetComponent<SpriteRenderer>().sprite = openedChest;
        
        if (opened == false)
        {
            FindObjectOfType<AudioManager>().Play("openChest");
            GetComponent<Transform>().position = new Vector3(transform.position.x, transform.position.y + 0.45f, transform.position.z);

            for (int i = 0; i < Random.Range(10.0f, 15.0f); i++)
            {
                if (dropLeft)
                {
                    GameObject coin = (GameObject)Instantiate(GameManager.Instance.Coin, new Vector3(transform.position.x-2, transform.position.y-1), Quaternion.identity);
                }
                else
                {
                    GameObject coin = (GameObject)Instantiate(GameManager.Instance.Coin, new Vector3(transform.position.x +2, transform.position.y - 1), Quaternion.identity);
                }
            }
        }
        opened = true;
    }

}
