using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    private float fillAmount;

    [SerializeField]
    private Image bar;

    [SerializeField]
    private GameObject barLength;

    private float xPosition;

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
        xPosition = barLength.GetComponent<RectTransform>().localPosition.x;
        //hp = transform.GetChild(0).GetComponent<Slider>();
        //mp = transform.GetChild(1).GetComponent<Slider>();
        //stamina = transform.GetChild(2).GetComponent < Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleStatusBar();
        BarLevelUp();
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

    private void BarLevelUp()
    {
        RectTransform rt = barLength.GetComponent<RectTransform>();

        if (barLength.name == "HP")
        {
            rt.sizeDelta = new Vector2(Player.Instance.hp.MaxValue * 4, 20);
            rt.localPosition = new Vector3(xPosition + (Player.Instance.hp.MaxValue * 4 - 160)/2,rt.localPosition.y,0);
        }
        if (barLength.name == "MP")
        {
            rt.sizeDelta = new Vector2(Player.Instance.mana.MaxValue * 4, 20);
            rt.localPosition = new Vector3(xPosition + (Player.Instance.mana.MaxValue * 4 - 160) / 2, rt.localPosition.y, 0);
        }
        if (barLength.name == "Stamina")
        {
            rt.sizeDelta = new Vector2(Player.Instance.stamina.MaxValue * 4, 20);
            rt.localPosition = new Vector3(xPosition + (Player.Instance.stamina.MaxValue * 4 - 160) / 2, rt.localPosition.y, 0);
        }

    }
}
