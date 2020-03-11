using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private float fillAmount;

    [SerializeField]
    private Image bar;

    //private Slider mp;
    //private Slider stamina;

    public float MaxBarValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, MaxBarValue);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //hp = transform.GetChild(0).GetComponent<Slider>();
        //mp = transform.GetChild(1).GetComponent<Slider>();
        //stamina = transform.GetChild(2).GetComponent < Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleStatusBar();
    }

    private void HandleStatusBar()
    {
        if (fillAmount != bar.fillAmount)
        {
            bar.fillAmount  = Mathf.Lerp(bar.fillAmount, fillAmount, Time.deltaTime*3);
        }     
    }

    private float Map(float current, float max)
    {
        return (current / max);
    }
}
