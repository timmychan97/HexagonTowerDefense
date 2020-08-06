using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected List<Btn_MainMenu> btnList = new List<Btn_MainMenu>();
    public float animDuration = 0.5f;
    float startOpacity = 0.1f;
    float startWidth = 10f;
    float endOpacity;
    float endWidth;

    // Start is called before the first frame update

    protected void Start()
    {
        SetMemberVars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetMemberVars()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogWarning("No RectTransform component found on MainMenuPanel");
        }

        foreach (Btn_MainMenu b in GetComponentsInChildren<Btn_MainMenu>()) 
        {
            btnList.Add(b);
        }
        endWidth = rectTransform.sizeDelta.x;
        endOpacity = 1f;
        if (btnList.Count > 0) endOpacity = btnList[0].GetOpacity();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(AnimatedShow());
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void HideWithAnimation()
    {
        StartCoroutine(AnimatedHide());
    }

    protected float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    protected IEnumerator AnimatedShow()
    {
        // animation when showing this menu
        // expand from left to right, incrementing opactiy in the meantime

        float endWidth = rectTransform.sizeDelta.x;
        float h = rectTransform.sizeDelta.y;

        float time = 0;
        float duration = animDuration;
        while (time < duration)
        {
            float t = time / duration;
            SetOpacity(Lerp(startOpacity, endOpacity, t));
            rectTransform.sizeDelta = new Vector2(Lerp(startWidth, endWidth, t), h); 

            yield return null;
            time += Time.deltaTime;
        }
        rectTransform.sizeDelta = new Vector2(endWidth, h);
        SetOpacity(endOpacity);
    }

    protected IEnumerator AnimatedHide()
    {
        // animation when hiding this menu
        // Shrink from right to left, decrementing opactiy in the meantime

        float endWidth = rectTransform.sizeDelta.x;
        // float endOpacity = 1f;
        if (btnList.Count > 0) endOpacity = btnList[0].GetOpacity();

        float h = rectTransform.sizeDelta.y;

        // The only difference from AnimatedShow() is here: Lerp from end to start
        float time = 0;
        float duration = animDuration;
        while (time < duration)
        {
            float t = time / duration;
            SetOpacity(Lerp(endOpacity, startOpacity, t));
            rectTransform.sizeDelta = new Vector2(Lerp(endWidth, startWidth, t), h); 

            yield return null;
            time += Time.deltaTime;
        }
        gameObject.SetActive(false);
        rectTransform.sizeDelta = new Vector2(endWidth, h);
        SetOpacity(endOpacity);
    }

    protected void SetOpacity(float a)
    {
        foreach (var b in btnList)
        {
            b.SetOpacity(a);
        }
    }
}
