using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Status hp;

    public int attackDamage;

    public float defence;

    [SerializeField]
    protected Transform kunaiPoint;

    [SerializeField]
    protected float movementSpeed;

    public Vector2 startPosition;

    protected bool facingRight;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    [SerializeField]
    private List<string> damageSource;

    [SerializeField]
    private GameObject kunaiPrefab;

    public Animator MyAnimator { get; private set; }

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public abstract bool IsDead { get; }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    // Start is called before the first frame update
    public virtual void Start()
    {
        startPosition = transform.position;
        facingRight = true;
        MyAnimator = GetComponent<Animator>();
        hp.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public virtual void ThrowKunai(int value)
    {
        if (facingRight)
        {
            GameObject kunai = (GameObject)Instantiate(kunaiPrefab, kunaiPoint.position, Quaternion.Euler(new Vector3(0, 0, -90)));
            kunai.GetComponent<Kunai>().Initialize(Vector2.right);
        }
        else
        {
            GameObject kunai = (GameObject)Instantiate(kunaiPrefab, kunaiPoint.position, Quaternion.Euler(new Vector3(0, 0, 90)));
            kunai.GetComponent<Kunai>().Initialize(Vector2.left);
        }
    }

    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSource.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }

    
}
