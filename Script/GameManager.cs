using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject coin;

    public GameObject Coin
    {
        get
        {
            return coin;
        }

    }

    [SerializeField]
    private Text coinText;

    private int gold;

    public int Gold 
    { 
        get => gold;
        set
        {      
            coinText.text = value.ToString();
            this.gold = value;
        }
    }

    [SerializeField]
    private GameObject kunaiDrop;

    public GameObject KunaiDrop
    {
        get
        {
            return kunaiDrop;
        }
    }

    [SerializeField]
    private Text kunaiText;

    private int numberKunai;

    public int NumberKunai
    {
        get => numberKunai;
        set
        {
            kunaiText.text = value.ToString();
            this.numberKunai = value;
        }
    }

    [SerializeField]
    private GameObject healthPotionDrop;

    public GameObject HealthPotionDrop
    {
        get
        {
            return healthPotionDrop;
        }
    }

    [SerializeField]
    private GameObject manaPotionDrop;

    public GameObject ManaPotionDrop
    {
        get
        {
            return manaPotionDrop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
