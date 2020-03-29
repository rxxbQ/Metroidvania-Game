using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastingBar : MonoBehaviour
{
    [SerializeField]
    private Image castingBar;

    public Transform castIcon;

    public RectTransform iconSize;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public Coroutine castCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        castIcon = transform.GetChild(1);
        iconSize = transform.GetChild(1).GetComponent<RectTransform>();
        transform.gameObject.SetActive(true);
        castingBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Progress()
    {
        float rate = 1.0f / castTime;
        float progress = 0.0f;
        canvasGroup.alpha = 1;

        while (progress <= 1.0)
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        StopCasting();
    }

    public void StopCasting()
    {
        if (castCoroutine != null)
        {
            canvasGroup.alpha = 0;
            StopCoroutine(castCoroutine);
            castCoroutine = null;
        }   
    }
}
