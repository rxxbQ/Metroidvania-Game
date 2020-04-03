using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = true;
        animator.SetFloat("speed", 0);

        if (animator.tag == "Player")
        {
            Player.Instance.restoreStamina = false;
            if (animator.GetBool("throw") == false)
            {
                Player.Instance.stamina.CurrentValue -= 20;
                FindObjectOfType<AudioManager>().Play("attack");
            }

            if (Player.Instance.OnGround)
            {
                Player.Instance.MyRigidbody.velocity = Vector2.zero;
            }
            else
            {
                Player.Instance.MyRigidbody.velocity = new Vector2(0, Player.Instance.MyRigidbody.velocity.y);
            }
        }
        if (animator.tag == "Enemy")
        {
            if (animator.GetBool("throw") == false)
            {
                FindObjectOfType<AudioManager>().Play("enemyAttack");
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = false;
        animator.GetComponent<Character>().SwordCollider.enabled = false;
        animator.ResetTrigger("attack");
        animator.SetBool("throw", false);

        if (animator.tag == "Player")
        {
            Player.Instance.restoreStamina = true;
        }   
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
