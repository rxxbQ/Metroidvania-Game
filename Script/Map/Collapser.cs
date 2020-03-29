using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (gameObject.tag == "HiddenRoom")
            {
                FindObjectOfType<AudioManager>().Play("hiddenRoom");
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("collapser");
            }
            Destroy(gameObject);
        }
    }
}
