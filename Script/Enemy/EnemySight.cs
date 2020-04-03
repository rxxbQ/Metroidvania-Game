using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;
    private bool sightBlocked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (enemy.Target == null && other.tag == "Obstacle")
        {
            sightBlocked = true;
        }
        if (!sightBlocked && other.tag == "Player")
        {
            enemy.Target = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            enemy.Target = null;
        }
        if (other.tag == "Obstacle")
        {
            sightBlocked = false;
        }
    }
}
