using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    protected RectTransform rectTransform;
    List<Btn_MainMenu> btnList = new List<Btn_MainMenu>();
    public float animDuration = 0.5f;
    // Start is called before the first frame update
    protected void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        Debug.Log("Show", this);
        gameObject.SetActive(true);
        StartCoroutine(ShowSelf(animDuration));
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    protected float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    protected IEnumerator ShowSelf(float duration)
    {
        Debug.Log("Show");
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) yield return null;

        float startWidth = 0;
        float h = rectTransform.sizeDelta.y;
        float endWidth = rectTransform.sizeDelta.x;
        float startOpacity = 0f;
        float endOpacity = 1f;

        float time = 0.0f;
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

    protected void SetOpacity(float a)
    {
        foreach (var b in btnList)
        {
            b.SetOpacity(a);
        }
    }
}
