using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_MainMenu : MonoBehaviour
{
    public Text text;
    
    void Start()
    {
        text = GetComponentInChildren<Text>();

        if (text == null) 
        {
            Debug.LogWarning("No Text component in children of Btn_LoadLevel");
        }

    }

    public void SetOpacity(float a)
    {
        Image img = GetComponent<Image>();
        img.color = new Color(1f, 1f, 1f, a);
        text.color = new Color(1f, 1f, 1f, a);
    }

    public void SetText(string s)
    {
        text.text = s;
    }
}
