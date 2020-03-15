using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private bool horizontal;

    private Vector3 startPos;
    private Vector3 downPos;
    private Vector3 upPos;
    private Vector3 leftPos;
    private Vector3 rightPos;

    private Vector3 nextPos;

    [SerializeField]
    private Transform downTransform;

    [SerializeField]
    private Transform upTransform;

    [SerializeField]
    private Transform leftTransform;

    [SerializeField]
    private Transform rightTransform;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;

        if (!horizontal)
        {
            downPos = downTransform.localPosition;
            upPos = upTransform.localPosition;
            nextPos = downPos;
        }
        else
        {
            leftPos = leftTransform.localPosition;
            rightPos = rightTransform.localPosition;
            nextPos = leftPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();   
    }

    private void Move()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, nextPos)<= 0.1)
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        if (!horizontal)
        {
            if(nextPos != upPos)
            {
                nextPos = upPos;
            }
            else
            {
                nextPos = downPos;
            }
        }
        else
        {
            if(nextPos != rightPos)
            {
                nextPos = rightPos;
            }
            else
            {
                nextPos = leftPos;                    
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.SetParent(null);
        }
    }
}
