using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoor : MonoBehaviour
{
    [SerializeField]
    private List<string> damageSource;

    [SerializeField]
    private Sprite brokenDoor;

    private int endurance = 2;

    // Update is called once per frame
    void Update()
    {
        if(endurance == 1)
        {
            GetComponent<SpriteRenderer>().sprite = brokenDoor;
        }
        if (endurance <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSource.Contains(other.tag))
        {
            endurance--;
        }
    }
}
