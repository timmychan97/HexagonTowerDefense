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

    protected void Start()
    {
        SetMemberVars();
    }

    void SetMemberVars()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
            Debug.LogWarning("No RectTransform component found on MainMenuPanel");
        
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
        AnimatedShow();
    }

    public virtual void Hide() => gameObject.SetActive(false);
    public virtual void HideWithAnimation() => AnimatedHide();

    /// <summary>
    /// Animation when showing this menu. Expand from left to right, incrementing opactiy in the meantime
    /// </summary>
    protected void AnimatedShow()
    {
        StartCoroutine(AnimatedShow(false));
    }

    /// <summary>
    /// Animation when hiding this menu. Shrink from right to left, decrementing opactiy in the meantime
    /// </summary>
    protected void AnimatedHide()
    {
        StartCoroutine(AnimatedShow(true));
    }

    /// <summary>
    /// General animation for showing and hiding panels
    /// </summary>
    /// <param name="inverse">Run hide animation instead</param>
    protected IEnumerator AnimatedShow(bool inverse)
    {
        float endWidth = rectTransform.sizeDelta.x;
        float h = rectTransform.sizeDelta.y;
        float time = 0;
        float duration = animDuration;

        if (inverse)
            if (btnList.Count > 0) endOpacity = btnList[0].GetOpacity();

        while (time < duration)
        {
            float t = time / duration;

            // If is running hide animation, reverse the lerp
            if (inverse) t = 1 - t;

            SetOpacity(Mathf.Lerp(startOpacity, endOpacity, t));
            rectTransform.sizeDelta = new Vector2(Mathf.Lerp(startWidth, endWidth, t), h);

            yield return null;
            time += Time.deltaTime;
        }

        if (inverse)
            gameObject.SetActive(false);

        rectTransform.sizeDelta = new Vector2(endWidth, h);
        SetOpacity(endOpacity);
    }

    protected void SetOpacity(float a) => btnList.ForEach(b => b.SetOpacity(a));
}
