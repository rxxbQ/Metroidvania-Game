using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Sprite lit;

    public void Interact()
    {
        Player.Instance.startPosition = transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = lit;
    }
}
