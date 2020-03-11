using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private static Enemy instance;
    public static Enemy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Enemy>();
            }
            return instance;
        }
    }

    private IEnemyState currentState;

    private Canvas healthCanvas;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    private bool dropItem = true;

    public GameObject Target { get; set; }

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private float throwRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }
            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return hp.CurrentValue <= 0;
        }
    }

    private float healthPotionChance = 0.3f;
    private float manaPotionChance = 0.1f;
    private float kunaiChance = 0.2f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        healthPotionChance += 0.01f * Player.Instance.Luck;
        manaPotionChance += 0.01f * Player.Instance.Luck;
        kunaiChance += 0.01f * Player.Instance.Luck;
        healthCanvas = transform.GetComponentInChildren<Canvas>();
        Player.Instance.Dead += new HandleDeadEvent(RemoveTarget);
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter(this);
    }

    public override void ChangeDirection()
    {
        Transform healthBar = transform.Find("EnemyHealthCanvas");
        Vector2 pos = healthBar.position;

        healthBar.SetParent(null);

        base.ChangeDirection();

        healthBar.SetParent(transform);

        healthBar.position = pos;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x ) ||
               (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
            }
            else if( currentState is PatrolState)
            {
                ChangeDirection();
            }
            else if (currentState is ThrowState)
            {
                Target = null;
                Player.Instance.InBattle = false;
                ChangeState(new IdleState());
            }
            
        }
        
    }

    public Vector2 GetDirection()
    {
        if (facingRight)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            Player.Instance.InBattle = true;
            float xDir = Target.transform.position.x - transform.position.x;
            
            if (xDir > 0 && !facingRight || xDir < 0 && facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void RemoveTarget()
    {
        Target = null;
        Player.Instance.InBattle = false;
        ChangeState(new PatrolState());
    }

    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }

        hp.CurrentValue = hp.CurrentValue - (int)Mathf.Round(Player.Instance.attackDamage * (1 - (0.01f * defence)));

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            if (dropItem)
            {
                GameObject coin = (GameObject)Instantiate(GameManager.Instance.Coin, new Vector3(transform.position.x, transform.position.y - 1), Quaternion.identity);
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                if (Random.Range(0f, 1f) < kunaiChance)
                {
                    GameObject kunaiDrop = (GameObject)Instantiate(GameManager.Instance.KunaiDrop, new Vector3(transform.position.x, transform.position.y - 1), Quaternion.Euler(new Vector3(0, 0, -90)));
                    Physics2D.IgnoreCollision(kunaiDrop.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                if (Random.Range(0f, 1f) < healthPotionChance)
                {
                    GameObject healthPotionDrop = (GameObject)Instantiate(GameManager.Instance.HealthPotionDrop, new Vector3(transform.position.x, transform.position.y - 1.5f), Quaternion.identity);
                    Physics2D.IgnoreCollision(healthPotionDrop.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                if (Random.Range(0f, 1f) < manaPotionChance)
                {
                    GameObject manaPotionDrop = (GameObject)Instantiate(GameManager.Instance.ManaPotionDrop, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
                    Physics2D.IgnoreCollision(manaPotionDrop.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                }
                dropItem = false;
            }
           
            MyAnimator.SetTrigger("die");
            yield return null;
        } 
    }

    public override void Death()
    {
        dropItem = true;
        healthCanvas.enabled = false;
        Destroy(gameObject);
        //MyAnimator.ResetTrigger("die");
        //MyAnimator.ResetTrigger("idle");
        //hp.CurrentValue = hp.MaxValue;
        //transform.position = startPosition;
    }
}
