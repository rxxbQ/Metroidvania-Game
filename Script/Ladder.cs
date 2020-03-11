using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteraction
{
    [SerializeField]
    private Collider2D platformCollider;

    public void Interact()
    {
        Player.Instance.OnLadder = true;
        Player.Instance.MyRigidbody.gravityScale = 0;
        Player.Instance.MyAnimator.SetLayerWeight(2, 1);
        Player.Instance.MyAnimator.speed = 0;
        //Player.Instance.MyAnimator.SetTrigger("reset");
        Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player.Instance.OnLadder = false;
            Player.Instance.MyRigidbody.gravityScale = 2;
            Player.Instance.MyAnimator.SetLayerWeight(2, 0);
            Player.Instance.MyAnimator.speed = 1;
            //Player.Instance.MyAnimator.ResetTrigger("reset");
            Physics2D.IgnoreCollision(Player.Instance.GetComponent<Collider2D>(), platformCollider, false);
        }
    }
}
