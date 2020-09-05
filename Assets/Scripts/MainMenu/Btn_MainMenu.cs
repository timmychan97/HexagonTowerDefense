using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_MainMenu : MonoBehaviour
{
    public Text text;
    Image img;

    void Awake()
    {
        text = GetComponentInChildren<Text>();
        img = GetComponent<Image>();
    }
    
    protected void Start()
    {
        // text = GetComponentInChildren<Text>();
        // img = GetComponent<Image>();

        if (!text) 
            Debug.LogWarning("No Text component in children of Btn_LoadLevel");

    }

    public void SetOpacity(float a)
    {
        img.color = new Color(1f, 1f, 1f, a);
        text.color = new Color(1f, 1f, 1f, a);
    }

    public float GetOpacity() => img.color.a;

    public void SetText(string s) => text.text = s;
}
