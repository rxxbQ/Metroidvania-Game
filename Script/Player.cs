using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void HandleDeadEvent();

public class Player : Character
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform upItem;
    public int upIndex = 0;

    [SerializeField]
    private Transform downItem;
    public int downIndex = 0;

    [SerializeField]
    private Transform leftItem;
    public int leftIndex = 0;

    [SerializeField]
    private Transform rightItem;
    public int rightIndex = 0;

    [SerializeField]
    private Transform castingBar;

    public event HandleDeadEvent Dead;

    private IInteraction usable;

    private IInteraction savePoint;

    private float groundRadius =0.2f;

    private float buttonMoveSpeed;
    private float buttonMove;
    private bool button;

    public bool immortal = false;

    private SpriteRenderer spriteRenderer;

    public Status mana;

    public Status stamina;

    public bool restoreStamina;

    private bool staminaEmpty;

    [SerializeField]
    private float immortalTime;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float climbSpeed;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private Transform[] groundPoints;

    public Rigidbody2D MyRigidbody { get; set; }

    public bool OnLadder { get; set; }
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (hp.CurrentValue <= 0)
            {
                OnDead();
            }
            
            return hp.CurrentValue <= 0;
        }
    }

    [SerializeField]
    private GameObject healthButton;
    [SerializeField]
    private GameObject manaButton;
    [SerializeField]
    private GameObject staminaButton;
    [SerializeField]
    private GameObject attackButton;
    [SerializeField]
    private GameObject defenceButton;
    [SerializeField]
    private GameObject luckButton;

    private int level;

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            this.level = value;
        }
    }

    private float luck;

    public float Luck
    {
        get
        {
            return luck;
        }
        set
        {
            this.luck = value;
        }
    }

    [SerializeField]
    private Text levelText;

    [SerializeField]
    private Text coinText;

    private int requiredCoin;

    public int RequiredCoin
    {
        get
        {
            return requiredCoin;
        }
        set
        {
            this.requiredCoin = value;
        }
    }

    [SerializeField]
    private Text requiredCoinText;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text manaText;

    [SerializeField]
    private Text staminaText;

    [SerializeField]
    private Text attackText;

    [SerializeField]
    private Text defenceText;

    [SerializeField]
    private Text luckText;

    private bool inBattle;

    public bool InBattle
    {
        get
        {
            return inBattle;
        }
        set
        {
            this.inBattle = value;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        staminaEmpty = false;
        restoreStamina = true;
        level = 0;
        requiredCoin = 10;
        luck = 10.0f;
        GameManager.Instance.NumberKunai = 10;
        OnLadder = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D>();

        mana.Initialize();
        stamina.Initialize();
    }

    // Update is called once per frame
    void Update()
    { 
        if (!TakingDamage && !IsDead)
        {
            if(transform.position.y <= -10f)
            {
                Death();
            }
            int previousUpItem = upIndex;
            int previousDownItem = downIndex;
            int previousLeftItem = leftIndex;
            int previousRightItem = rightIndex;

            HandleInput();
            
            if (previousUpItem != upIndex)
            {
                upItem.GetComponent<SwitchItem>().SwitchItems(upIndex);
            }
            if (previousDownItem != downIndex)
            {
                downItem.GetComponent<SwitchItem>().SwitchItems(downIndex);
            }
            if (previousLeftItem != leftIndex)
            {
                leftItem.GetComponent<SwitchItem>().SwitchItems(leftIndex);
            }
            if (previousRightItem != rightIndex)
            {
                rightItem.GetComponent<SwitchItem>().SwitchItems(rightIndex);
            }

            if (GameManager.Instance.Gold < requiredCoin || inBattle)
            {
                healthButton.SetActive(false);
                manaButton.SetActive(false);
                staminaButton.SetActive(false);
                attackButton.SetActive(false);
                defenceButton.SetActive(false);
                luckButton.SetActive(false);
            }
            else
            {
                healthButton.SetActive(true);
                manaButton.SetActive(true);
                staminaButton.SetActive(true);
                attackButton.SetActive(true);
                defenceButton.SetActive(true);
                luckButton.SetActive(true);
            }

            levelText.text = level.ToString();
            coinText.text = GameManager.Instance.Gold.ToString();
            requiredCoinText.text = requiredCoin.ToString();
            healthText.text = hp.MaxValue.ToString();
            manaText.text = mana.MaxValue.ToString();
            staminaText.text = stamina.MaxValue.ToString();
            attackText.text = attackDamage.ToString();
            defenceText.text = defence.ToString();
            luckText.text = luck.ToString();
        }
    }

    void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (restoreStamina)
            {
                stamina.CurrentValue += 0.3f;
                if (stamina.CurrentValue == stamina.MaxValue)
                {
                    staminaEmpty = false;
                }
            }
            if (stamina.CurrentValue <=0)
            {
                staminaEmpty = true;
            }

            if (Mathf.Abs(horizontal) > 0)
            {
                castingBar.GetComponent<CastingBar>().StopCasting();
            }

            OnGround = IsGrounded();

            HandleLayer();

            if (button)
            {
                buttonMoveSpeed = Mathf.Lerp(buttonMoveSpeed, buttonMove, Time.deltaTime * 3);
                //HandleMovement(buttonMoveSpeed);
                Flip(buttonMove);
            }
            else
            {
                HandleMovement(horizontal,vertical);
                Flip(horizontal);
            }
        }   
    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.L) && mana.CurrentValue > 0)
        {
            mana.CurrentValue -= 20;
            GameManager.Instance.NumberKunai += 2;
        }
        if (Input.GetKeyDown(KeyCode.J) && !OnLadder && !staminaEmpty)
        {
            castingBar.GetComponent<CastingBar>().StopCasting();
            MyAnimator.SetTrigger("attack");
            stamina.CurrentValue -= 20;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && !MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !OnLadder)
        {
            MyAnimator.SetTrigger("slide");
            stamina.CurrentValue -= 15;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !OnLadder)
        {
            castingBar.GetComponent<CastingBar>().StopCasting();
            MyAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.K) && !OnLadder && GameManager.Instance.NumberKunai >0)
        {
            castingBar.GetComponent<CastingBar>().StopCasting();
            MyAnimator.SetTrigger("throw");
            stamina.CurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            castingBar.GetComponent<CastingBar>().StopCasting();
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (upIndex >= upItem.childCount - 1)
            {
                upIndex = 0;
            }
            else
            {
                upIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (downIndex >= downItem.childCount - 1)
            {
                downIndex = 0;
            }
            else
            {
                downIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (leftIndex >= leftItem.childCount - 1)
            {
                leftIndex = 0;
            }
            else
            {
                leftIndex++;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (rightIndex >= rightItem.childCount - 1)
            {
                rightIndex = 0;
            }
            else
            {
                rightIndex++;
            }
        }
    }

    private void HandleLayer()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    private void HandleMovement(float horizontal, float vertical)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide && OnGround)
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if (Jump && MyRigidbody.velocity.y == 0 && !OnLadder)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        if (OnLadder)
        {
            MyAnimator.speed = Mathf.Abs(vertical);
            MyRigidbody.velocity = new Vector2(horizontal * climbSpeed, vertical * climbSpeed);
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void Flip(float horizontal)
    {
        if (!MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
                ChangeDirection();
            }
        }
    }

    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i=0; i<colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public override void ThrowKunai(int value)
    {
        if (!OnGround && value ==1 || OnGround && value == 0)
        {
            base.ThrowKunai(value);
            GameManager.Instance.NumberKunai--;
        }
    }

    private void Interact()
    {
        if (usable != null)
        {
            usable.Interact();
        }
        if (savePoint != null)
        {
            savePoint.Interact();
        }
    }

    //private IEnumerator Immortal()
    //{
    //    while (immortal)
    //    {
    //        spriteRenderer.enabled = false;

    //        yield return new WaitForSeconds(0.1f);

    //        spriteRenderer.enabled = true;

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            hp.CurrentValue = hp.CurrentValue - (int)Mathf.Round(Enemy.Instance.attackDamage * (1 - (0.01f* defence)));

            if (!IsDead)
            {
                castingBar.GetComponent<CastingBar>().StopCasting();
                MyAnimator.SetTrigger("damage");
                immortal = true;
                //StartCoroutine(Immortal());

                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
            }
        }    
    }

    public override void Death()
    {
        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        hp.CurrentValue = 0.5f * hp.MaxValue;
        GameManager.Instance.Gold = (int)Mathf.Round(GameManager.Instance.Gold * 0.5f);
        transform.position = startPosition;
    }

    public void ButtonMove(float buttonMove)
    {
        this.buttonMove = buttonMove;
        button = true;
    }

    public void ButtonStopMove()
    {
        buttonMove = 0;
        buttonMoveSpeed = 0;
        button = false;
    }

    public void ButtonJump()
    {
        MyAnimator.SetTrigger("jump");
    }

    public void ButtonSlide()
    {
        MyAnimator.SetTrigger("slide");
    }

    public void ButtonAttack()
    {
        MyAnimator.SetTrigger("attack");
    }

    public void ButtonThrow()
    {
        MyAnimator.SetTrigger("throw");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            GameManager.Instance.Gold+=100;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "KunaiDrop")
        {
            GameManager.Instance.NumberKunai += 1;
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Ceiling")
        {
            MyAnimator.SetBool("land", true);
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Usable")
        {
            usable = other.GetComponent<IInteraction>();
        }
        if (other.tag == "SavePoint")
        {
            savePoint = other.GetComponent<IInteraction>();
        }
        if (other.tag == "Platform")
        {
            MyAnimator.SetBool("land", true);
        }
        base.OnTriggerEnter2D(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Usable")
        {
            usable = null;
        }
        if (other.tag == "SavePoint")
        {
            savePoint = null;
        }
    }
}
