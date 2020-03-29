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
        Player.Instance.hp.CurrentValue = Player.Instance.hp.MaxValue;
        Player.Instance.mana.CurrentValue = Player.Instance.mana.MaxValue;
        Player.Instance.stamina.CurrentValue = Player.Instance.stamina.MaxValue;

        FindObjectOfType<AudioManager>().Play("save");
    }
}
