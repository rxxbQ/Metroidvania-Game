﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBehaviour : StateMachineBehaviour
{
    //private BoxCollider2D boxCollider;

    //private Vector2 slideSize = new Vector2(1.79f, 2);
    //private Vector2 slideOffset = new Vector2(0, -1.3f);

    //private Vector2 size;
    //private Vector2 offset;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Slide = true;
        Player.Instance.immortal = true;
        Player.Instance.restoreStamina = false;

        FindObjectOfType<AudioManager>().Play("slide");

        //if (boxCollider == null)
        //{
        //    boxCollider = Player.Instance.GetComponent<BoxCollider2D>();
        //    size = boxCollider.size;
        //    offset = boxCollider.offset;
        //}

        //boxCollider.size = slideSize;
        //boxCollider.offset = slideOffset;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Slide = false;
        animator.ResetTrigger("slide");
        Player.Instance.immortal = false;
        Player.Instance.restoreStamina = true;
        
        //boxCollider.size = size;
        //boxCollider.offset = offset;
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
